using Domain.Core.Dtos;
using Domain.Generic;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Domain.Core
{
    public class User : IdentityUser<int>, IEntity<UserDto>
    {
        public bool ChangePassword { get; private set; }
        public bool IsSuspended { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        // 1-1 Owned entity relationship between User and Profile. Profile can't exist exist without User
        public virtual Profile Profile { get; set; }

        // M-M 
        protected virtual List<IdentityUserRole<int>> RoleUsers { get; private set; }
        public IReadOnlyCollection<IdentityUserRole<int>> ReadOnlyRoleUsers => RoleUsers;

        public User() : base()
        {
            RoleUsers = new List<IdentityUserRole<int>>();
        }

        public void AddRoles(params Role[] roles)
        {
            foreach(Role role in roles)
            {
                var roleUser = new IdentityUserRole<int>() { RoleId = role.Id, UserId = Id };
                RoleUsers.Add(roleUser);
            }
        }

        public UserDto ToDto() => new UserDto()
        {
            Slug = Id.ToString(),
            UserName = UserName,
            Email = Email,
            IsSuspended = IsSuspended,
            CreatedAt = CreatedAt != null ? Formatter.Format(CreatedAt) : null,
            UpdatedAt = UpdatedAt != null ? Formatter.Format(UpdatedAt) : null
        };
    }

    public class Profile
    {
        [PersonalData]
        public string FirstName { get; set; }

        [PersonalData]
        public string LastName { get; set; }
        
        private string gender;
        public Gender? Gender
        {
            get => string.IsNullOrEmpty(gender) ? null as Gender? : Enum.Parse<Gender>(gender);
            set => gender = value?.ToString();
        }

        public DateTime DateOfBirth { get; set; }

        public ProfileDto ToDto() => new ProfileDto()
        {
            FirstName = FirstName,
            LastName = LastName,
            Gender = gender,
            DateOfBirth = DateOfBirth != null ? Formatter.Format(DateOfBirth) : null
        };
    }

    public enum Gender
    {
        Male,
        Female
    }
}
