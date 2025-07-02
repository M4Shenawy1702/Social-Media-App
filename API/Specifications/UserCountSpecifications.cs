internal class UserCountSpecifications : BaseSpecifications<ApplicationUser>
{
    public UserCountSpecifications(UserQueryParameters parameters, string currentUserId)
        : base(u =>
            (string.IsNullOrWhiteSpace(parameters.SearchByName) || u.DisplayName.ToLower().Trim().Contains(parameters.SearchByName.ToLower().Trim())) &&
            u.Id != currentUserId
        )
    {
    }
}
