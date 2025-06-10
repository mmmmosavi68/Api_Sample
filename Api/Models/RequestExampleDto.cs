using Swashbuckle.AspNetCore.Filters;

namespace Api.Models
{
    public class RequestExampleDto
    {
        public class CreateUserRequestExample : IExamplesProvider<UserDto>
        {
            public UserDto GetExamples()
            {
                return new UserDto
                {
                    FullName = "میرمحمد موسوی",
                    Age = 25,
                    UserName = "Mousavi",
                    Email = "Mousavi@site.com",
                    Gender = Entities.GenderType.Male,
                    Password = "1234567"
                };
            }
        }
        public class CreateUserResponseExample : IExamplesProvider<UserDto>
        {
            public UserDto GetExamples()
            {
                return new UserDto
                {
                    FullName = "میرمحمد موسوی",
                    Age = 25,
                    UserName = "Mousavi",
                    Email = "Mousavi@site.com",
                    Gender = Entities.GenderType.Male,
                    Password = "1234567"
                };
            }
        }
    }
}
