using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;


namespace ArsalanApp.DTOs
{
    /// <summary>
    /// DTO for user registration.
    /// </summary>
    public class RegistrationDto
    {
        /// <summary>
        /// The username of the user.
        /// </summary>
        [SwaggerSchema(Description = "The username of the user.")]
        [DefaultValue("abc")]
        public string UserName { get; set; } = "abc";

        /// <summary>
        /// The email address of the user.
        /// </summary>
        [SwaggerSchema(Description = "The email address of the user.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// The password for the account.
        /// </summary>
        [SwaggerSchema(Description = "The password for the account.")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// The contact number of the user.
        /// </summary>
        [SwaggerSchema(Description = "The contact number of the user.")]
        [DefaultValue("03325294773")]
        public string ContactNo { get; set; } = string.Empty;

        /// <summary>
        /// The date of birth of the user.
        /// </summary>
        [SwaggerSchema(Description = "The date of birth of the user. Default Value = 2000-01-01")]
        public DateOnly DateOfBirth { get; set; } = new DateOnly();

        public int Id { get => field; set; }

        


    private class MyClass
    {
        private int field;
        public int Property
        {
            get => field;
            set => field = value;
        }

        public MyClass myClass { get; set; }

    }

    }


}