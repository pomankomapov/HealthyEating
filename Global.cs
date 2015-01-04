using System;

namespace healthy_eating
{
    // Глобальное пространство переменных
    //  доступно из любого активити

    public static class Global
    {
        public static int userID { get; set; }       // ID пользователя этого устройства
        public static string deviceID { get; set; }  // ID устройства
    }
}

