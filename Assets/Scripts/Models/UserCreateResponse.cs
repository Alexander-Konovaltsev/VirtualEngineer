using System;

namespace VirtualEngineer.Models
{
    public class UserCreateResponse
    {
        public int id;
        public string last_name;
        public string first_name;
        public string patronymic;
        public string email;
        public int role_id;
        public string workplace;
        public DateTime created_at;
    }
}