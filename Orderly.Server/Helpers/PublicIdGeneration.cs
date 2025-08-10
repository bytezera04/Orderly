
using Orderly.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace Orderly.Shared.Helpers
{
    public class PublicIdGeneration
    {
        private static Random Rnd = new Random();

        public static async Task<string> GenerateUserId(AppDbContext context)
        {
            string code;
            do
            {
                code = GeneratePublicId();
            } while (await context.Users.AnyAsync(u => u.PublicId == code));

            return code;
        }

        public static async Task<string> GenerateProductId(AppDbContext context)
        {
            string code;
            do
            {
                code = GeneratePublicId();
            } while (await context.Products.AnyAsync(u => u.PublicId == code));

            return code;
        }

        public static async Task<string> GenerateOrderId(AppDbContext context)
        {
            string code;
            do
            {
                code = GeneratePublicId();
            } while (await context.Orders.AnyAsync(u => u.PublicId == code));

            return code;
        }

        public static async Task<string> GenerateChatThreadId(AppDbContext context)
        {
            string code;
            do
            {
                code = GeneratePublicId();
            } while (await context.ChatThreads.AnyAsync(u => u.PublicId == code));

            return code;
        }

        public static async Task<string> GenerateChatMessageId(AppDbContext context)
        {
            string code;
            do
            {
                code = GeneratePublicId();
            } while (await context.ChatMessages.AnyAsync(u => u.PublicId == code));

            return code;
        }

        private static string GeneratePublicId()
        {
            const int PUBLIC_ID_LENGTH = 8;

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, PUBLIC_ID_LENGTH)
                .Select(s => s[Rnd.Next(s.Length)]).ToArray());
        }
    }
}
