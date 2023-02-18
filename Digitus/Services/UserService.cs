using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Digitus.Dtos;
using Digitus.Models;
using Digitus.Settings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Digitus.Services;

public class UserService : IUserService
{
    private readonly IMongoCollection<User> _signupCollection;
    private readonly IMongoCollection<Login> _loginCollection;
    private readonly IMapper _mapper;

    public UserService(IMapper mapper,IDatabaseSetting databaseSetting)
    {
        var client = new MongoClient(databaseSetting.ConnectionString);
        var database = client.GetDatabase(databaseSetting.DatabaseName);
        _signupCollection = database.GetCollection<User>(databaseSetting.SignupCollectionName);
        _loginCollection = database.GetCollection<Login>(databaseSetting.LoginCollectionName);
        _mapper = mapper;
    }

    public async Task<Response<User>> Register(SignupDto signupDto)
    {
        var existUser = await _signupCollection.FindAsync(x => x.Email == signupDto.Email);
        if (await existUser.AnyAsync()) return Response<User>.Fail("User already exist", 500);
        byte[] passwordHash = Array.Empty<byte>(), passwordSalt = Array.Empty<byte>();
        if (signupDto.Password != null)
            CreatedPasswordHash(signupDto.Password, out passwordHash,out passwordSalt);
        var user = new User()
        {
            Email = signupDto.Email,
            LastName = signupDto.LastName,
            FirstName = signupDto.FirstName,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            VerificationCode = CreateRandomCode(),

        };
        await _signupCollection.InsertOneAsync(user);

        return Response<User>.Success(user,200);

    }

    public async Task<Response<NoContent>> DeleteUser(string id)
    {
        var user = await _signupCollection.Find(x => x.Id == id).FirstAsync();
        if (user == null) return Response<NoContent>.Fail("User not found", 404);
        await _signupCollection.DeleteOneAsync(x=>x.Id==id);
        return Response<NoContent>.Success("User deleted is successfully",204);
    }

    public async Task<Response<NoContent>> Verify(string code)
    {
        var user = await _signupCollection.Find(x => x.VerificationCode == code).FirstAsync();
        if (user == null) return Response<NoContent>.Fail("Invalid code", 500);
        user.VerifiedAt=DateTime.Now;
        await _signupCollection.FindOneAndReplaceAsync(x=>x.VerificationCode==code,user);
        return Response<NoContent>.Success("User is verified", 200);
        
    }

    public async Task<Response<NoContent>> ForgotPassword(string email)
    {
        var user = await _signupCollection.Find(x => x.Email == email).FirstAsync();
        if (user == null) return Response<NoContent>.Fail("User not found", 404);
        user.PasswordResetCode = CreateRandomCode();
        await _signupCollection.FindOneAndReplaceAsync(x=>x.Email == email, user);
        return Response<NoContent>.Success($"You can reset password this code: {user.PasswordResetCode}", 200);

    }

    
    public async Task<Response<NoContent>> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        var user = _signupCollection.Find(x => x.PasswordResetCode == resetPasswordDto.PasswordResetCode).FirstAsync();
        if (user.IsCompletedSuccessfully == false) return Response<NoContent>.Fail("Code is incorrect", 404);
        byte[] passwordHash = Array.Empty<byte>(), passwordSalt = Array.Empty<byte>();
        if (resetPasswordDto.Password != null)
            CreatedPasswordHash(resetPasswordDto.Password, out passwordHash, out passwordSalt);
        user.Result.PasswordHash = passwordHash;
        user.Result.PasswordSalt = passwordSalt;
        await _signupCollection.FindOneAndReplaceAsync(x => x.PasswordResetCode == resetPasswordDto.PasswordResetCode,
            user.Result);
        return Response<NoContent>.Success("Password reset is successful", 200);
    }

    public async Task<Response<LoginDto>> Login(LoginDto loginDto)
    {
        var user = _signupCollection.Find(x => x.Email == loginDto.Email).FirstAsync();
        if (user.Result==null) return Response<LoginDto>.Fail("User is not registered", 500);
        if (user.Result.VerifiedAt==null)
        {
            return Response<LoginDto>.Fail("Please confirm your activation code", 500);
        }

        if (!VerifyPasswordHash(loginDto.Password!,user.Result.PasswordHash!,user.Result.PasswordSalt!))
        {
            return Response<LoginDto>.Fail("Password is incorrect", 500);
        }

        var login = new Login()
        {
            Email = user.Result.Email,
            IsAdmin = user.Result.IsAdmin,
            LoginStartTime = DateTime.Now
        };
        await _loginCollection.InsertOneAsync(login);
        return Response<LoginDto>.Success($"Welcome to Digitus {loginDto.Email}", 200);
    }

    public async Task<Response<LoginDto>> Logout(string email)
    {
        var login = await _loginCollection.Find(x => x.Email == email && x.LoginEndTime==null).FirstAsync();
        login.LoginEndTime=DateTime.Now;
            await _loginCollection.FindOneAndReplaceAsync(x=>x.Id==login.Id,login);
        return Response<LoginDto>.Success($"We are waiting again {login.Email}", 200);
    }

    public async Task<Response<List<Login>>> GetLoginUsers()
    {
       var logins = await _loginCollection.Find(x=>x.LoginEndTime==null ).ToListAsync();
        return Response<List<Login>>.Success(logins,200);

    }

    public async Task<Response<List<Login>>> GetUserLogins(string email)
    {
        var logins = await _loginCollection.Find(x => x.Email == email).ToListAsync();
        return logins != null
            ? Response<List<Login>>.Success(logins, 200)
            : Response<List<Login>>.Fail("This user has not logged in before", 404);
    }

    public async Task<Response<NoContent>> GetUserLoginTime(string email)
    {
        var login =await  _loginCollection.Find(x => x.Email == email).SortByDescending(x=>x.LoginStartTime).Limit(1).FirstAsync();
        if (login.LoginEndTime == null) login.LoginEndTime = Convert.ToDateTime(DateTime.Now);
        var time = Convert.ToDateTime(login.LoginEndTime) - Convert.ToDateTime(login.LoginStartTime);
        return Response<NoContent>.Success($"{time.Days.ToString()} :{time.Hours.ToString()}:{time.Minutes}", 200);
        
    }
    

    private void CreatedPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA256();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
    
    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac=new HMACSHA256(passwordSalt);
        var computeHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computeHash.SequenceEqual(passwordHash);
    }

    private string CreateRandomCode()
    {
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
    }

   
}