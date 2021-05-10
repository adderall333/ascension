namespace Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int AuthorizedUserId { get; set; }

        public Cart(int authorizedUserId)
        {
            AuthorizedUserId = authorizedUserId;
        }

        public Cart()
        {
        }
    }
}