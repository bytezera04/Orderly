
using Orderly.Server.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Orderly.Shared.Helpers;

namespace Orderly.Server.Data
{
    public static class DbSeeder
    {
        private static Random Rnd = new Random();

        public static async Task SeedTestDataAsync(IServiceProvider provider)
        {
            //
            //   SEED TEST DATA
            //
            //  - Run only once (not designed to be ran twice without clearing tables)
            //  - Clear all tables before running
            //

            using var scope = provider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // get the seed password

            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            string seedPassword = config.GetValue<string>("SeedPassword");

            // Seed Users

            var usersData = new[]
            {
                new { Name = "John Smith", Address = "123 Main St, London, UK", Email = "john.smith@example.com" },
                new { Name = "Emily Johnson", Address = "456 High St, London, UK", Email = "emily.johnson@example.com" },
                new { Name = "Michael Brown", Address = "789 Oak Ave, London, UK", Email = "michael.brown@example.com" },
                new { Name = "Olivia Williams", Address = "101 Maple Rd, London, UK", Email = "olivia.williams@example.com" },
                new { Name = "Daniel Jones", Address = "202 Park Ln, London, UK", Email = "daniel.jones@example.com" },
                new { Name = "Sophia Miller", Address = "303 Cedar St, London, UK", Email = "sophia.miller@example.com" },
                new { Name = "James Davis", Address = "404 Elm Dr, London, UK", Email = "james.davis@example.com" },
                new { Name = "Isabella Garcia", Address = "505 Pine Ave, London, UK", Email = "isabella.garcia@example.com" },
                new { Name = "Ethan Martinez", Address = "606 Sunset Blvd, London, UK", Email = "ethan.martinez@example.com" },
                new { Name = "Mia Rodriguez", Address = "707 Lakeview Rd, London, UK", Email = "mia.rodriguez@example.com" },
                new { Name = "Alexander Hernandez", Address = "808 Church St, London, UK", Email = "alexander.hernandez@example.com" },
                new { Name = "Charlotte Wilson", Address = "909 King St, London, UK", Email = "charlotte.wilson@example.com" },
                new { Name = "Henry Moore", Address = "110 Queen St, London, UK", Email = "henry.moore@example.com" },
                new { Name = "Amelia Taylor", Address = "111 Mill Rd, London, UK", Email = "amelia.taylor@example.com" },
                new { Name = "Lucas Anderson", Address = "112 Broadway, London, UK", Email = "lucas.anderson@example.com" },
                new { Name = "Grace Thomas", Address = "113 North St, London, UK", Email = "grace.thomas@example.com" },
                new { Name = "Sebastian Lee", Address = "114 South St, London, UK", Email = "sebastian.lee@example.com" },
                new { Name = "Ava White", Address = "115 East Rd, London, UK", Email = "ava.white@example.com" },
                new { Name = "Benjamin Harris", Address = "116 West Ln, London, UK", Email = "benjamin.harris@example.com" },
                new { Name = "Lily Clark", Address = "117 Victoria Ave, London, UK", Email = "lily.clark@example.com" },
                new { Name = "David Lewis", Address = "118 Main St, London, UK", Email = "david.lewis@example.com" },
                new { Name = "Ella Walker", Address = "119 High St, London, UK", Email = "ella.walker@example.com" },
                new { Name = "Matthew Hall", Address = "120 Oak Ave, London, UK", Email = "matthew.hall@example.com" },
                new { Name = "Chloe Allen", Address = "121 Maple Rd, London, UK", Email = "chloe.allen@example.com" },
                new { Name = "Joseph Young", Address = "122 Park Ln, London, UK", Email = "joseph.young@example.com" },
                new { Name = "Zoe King", Address = "123 Cedar St, London, UK", Email = "zoe.king@example.com" },
                new { Name = "Samuel Wright", Address = "124 Elm Dr, London, UK", Email = "samuel.wright@example.com" },
                new { Name = "Madison Scott", Address = "125 Pine Ave, London, UK", Email = "madison.scott@example.com" },
                new { Name = "Nathan Green", Address = "126 Sunset Blvd, London, UK", Email = "nathan.green@example.com" },
                new { Name = "Samantha Baker", Address = "127 Lakeview Rd, London, UK", Email = "samantha.baker@example.com" }
            };

            List<AppUser> users = new List<AppUser>();

            foreach (var userData in usersData)
            {
                var user = new AppUser
                {
                    PublicId = await PublicIdGeneration.GenerateUserId(context),
                    UserName = userData.Email,
                    Email = userData.Email,
                    FullName = userData.Name,
                    Address = userData.Address,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(user, seedPassword);

                users.Add(user);
            }

            // Seed Products

            var categories = new[] { "Electronics", "Clothing", "Books", "Toys", "Home" };

            var electronicsProducts = new[]
            {
                new { Category = ProductCategory.Electronics, Name = "Wireless Bluetooth Headphones", Description = "Over-ear, noise-cancelling Bluetooth headphones with 20-hour battery life.", Price = 89.99m, Tags = new[] { "audio", "wireless", "bluetooth", "headphones" } },
                new { Category = ProductCategory.Electronics, Name = "Smartphone 128GB", Description = "Latest generation smartphone with high-res display and multi-camera setup.", Price = 699.00m, Tags = new[] { "phone", "smartphone", "android", "ios" } },
                new { Category = ProductCategory.Electronics, Name = "4K Ultra HD Smart TV", Description = "55-inch 4K UHD smart TV with HDR support and streaming apps.", Price = 599.99m, Tags = new[] { "tv", "4k", "smart", "ultrahd" } },
                new { Category = ProductCategory.Electronics, Name = "Portable Power Bank 20000mAh", Description = "High capacity power bank with fast charging for multiple devices.", Price = 29.95m, Tags = new[] { "powerbank", "portable", "charger", "usb" } },
                new { Category = ProductCategory.Electronics, Name = "Wireless Gaming Mouse", Description = "Ergonomic wireless mouse with customizable RGB lighting and high precision.", Price = 49.99m, Tags = new[] { "gaming", "mouse", "wireless", "rgb" } },
                new { Category = ProductCategory.Electronics, Name = "Mechanical Gaming Keyboard", Description = "RGB backlit mechanical keyboard with programmable keys and anti-ghosting.", Price = 89.50m, Tags = new[] { "keyboard", "gaming", "mechanical", "rgb" } },
                new { Category = ProductCategory.Electronics, Name = "Smart Home Speaker", Description = "Voice-controlled smart speaker with built-in assistant and smart home integration.", Price = 79.99m, Tags = new[] { "smart", "speaker", "home", "voice" } },
                new { Category = ProductCategory.Electronics, Name = "Action Camera 4K", Description = "Compact waterproof action camera with 4K video recording and image stabilization.", Price = 149.00m, Tags = new[] { "camera", "action", "4k", "waterproof" } },
                new { Category = ProductCategory.Electronics, Name = "Wireless Earbuds", Description = "True wireless earbuds with charging case and touch controls.", Price = 59.99m, Tags = new[] { "earbuds", "wireless", "bluetooth", "audio" } },
                new { Category = ProductCategory.Electronics, Name = "Smartwatch with Heart Rate Monitor", Description = "Fitness smartwatch with heart rate monitor, GPS, and customizable watch faces.", Price = 129.99m, Tags = new[] { "smartwatch", "fitness", "gps", "heart-rate" } },
                new { Category = ProductCategory.Electronics, Name = "External SSD 1TB", Description = "Portable external solid state drive with USB-C and fast read/write speeds.", Price = 139.99m, Tags = new[] { "ssd", "external", "storage", "usb-c" } },
                new { Category = ProductCategory.Electronics, Name = "Noise Cancelling Earphones", Description = "In-ear earphones with active noise cancellation and superior sound quality.", Price = 79.00m, Tags = new[] { "earphones", "noise-cancelling", "audio", "in-ear" } },
                new { Category = ProductCategory.Electronics, Name = "Digital DSLR Camera", Description = "Professional-grade DSLR camera with 24MP sensor and interchangeable lenses.", Price = 899.99m, Tags = new[] { "camera", "dslr", "photography", "professional" } },
                new { Category = ProductCategory.Electronics, Name = "4K Projector", Description = "Compact 4K projector with HDR support and wireless connectivity.", Price = 399.99m, Tags = new[] { "projector", "4k", "wireless", "hdr" } },
                new { Category = ProductCategory.Electronics, Name = "Smart Thermostat", Description = "WiFi-enabled thermostat with app control and energy-saving features.", Price = 149.95m, Tags = new[] { "smart", "thermostat", "wifi", "energy" } },
                new { Category = ProductCategory.Electronics, Name = "USB-C Hub with HDMI", Description = "Multiport USB-C hub featuring HDMI, USB 3.0, and SD card reader.", Price = 34.99m, Tags = new[] { "usb-c", "hub", "hdmi", "adapter" } },
                new { Category = ProductCategory.Electronics, Name = "Wireless Charging Pad", Description = "Qi-certified wireless charging pad compatible with smartphones and earbuds.", Price = 19.99m, Tags = new[] { "wireless", "charger", "qi", "pad" } },
                new { Category = ProductCategory.Electronics, Name = "VR Headset", Description = "Virtual reality headset with high resolution and comfortable design.", Price = 299.00m, Tags = new[] { "vr", "headset", "virtual-reality", "gaming" } },
                new { Category = ProductCategory.Electronics, Name = "Laptop Cooling Pad", Description = "USB-powered cooling pad with adjustable fan speed for laptops.", Price = 24.95m, Tags = new[] { "laptop", "cooling", "usb", "fan" } },
                new { Category = ProductCategory.Electronics, Name = "Smart Light Bulb", Description = "Color-changing smart LED light bulb with app and voice control.", Price = 14.99m, Tags = new[] { "smart", "light", "led", "color" } },
                new { Category = ProductCategory.Electronics, Name = "Bluetooth Speaker", Description = "Portable Bluetooth speaker with deep bass and water-resistant design.", Price = 49.90m, Tags = new[] { "speaker", "bluetooth", "portable", "waterproof" } },
                new { Category = ProductCategory.Electronics, Name = "Wireless Keyboard and Mouse Combo", Description = "Compact wireless keyboard and mouse set with long battery life.", Price = 39.99m, Tags = new[] { "keyboard", "mouse", "wireless", "combo" } },
                new { Category = ProductCategory.Electronics, Name = "Noise Cancelling Over-Ear Headphones", Description = "Comfortable over-ear headphones with active noise cancellation and long battery life.", Price = 99.99m, Tags = new[] { "headphones", "noise-cancelling", "wireless", "audio" } },
                new { Category = ProductCategory.Electronics, Name = "Portable Bluetooth Tracker", Description = "Small Bluetooth tracker for keys, bags, and other valuables.", Price = 19.95m, Tags = new[] { "tracker", "bluetooth", "portable", "finder" } },
                new { Category = ProductCategory.Electronics, Name = "Smart Doorbell Camera", Description = "WiFi-enabled doorbell with HD video camera and two-way audio.", Price = 129.00m, Tags = new[] { "doorbell", "camera", "smart", "security" } },
                new { Category = ProductCategory.Electronics, Name = "Gaming Monitor 27-inch", Description = "27-inch gaming monitor with 144Hz refresh rate and 1ms response time.", Price = 279.99m, Tags = new[] { "monitor", "gaming", "144hz", "display" } },
                new { Category = ProductCategory.Electronics, Name = "Wireless Network Adapter", Description = "USB WiFi adapter supporting latest 802.11ac standard.", Price = 25.99m, Tags = new[] { "wifi", "adapter", "wireless", "usb" } },
                new { Category = ProductCategory.Electronics, Name = "Smartphone Gimbal Stabilizer", Description = "Handheld gimbal for smooth video recording with smartphones.", Price = 119.95m, Tags = new[] { "gimbal", "stabilizer", "smartphone", "video" } },
                new { Category = ProductCategory.Electronics, Name = "Bluetooth Car Kit", Description = "Hands-free Bluetooth kit for car audio and calls.", Price = 35.00m, Tags = new[] { "bluetooth", "car", "hands-free", "audio" } },
                new { Category = ProductCategory.Electronics, Name = "Portable Solar Charger", Description = "Solar-powered charger suitable for outdoor use and emergency.", Price = 45.50m, Tags = new[] { "solar", "charger", "portable", "outdoor" } },
                new { Category = ProductCategory.Electronics, Name = "Dash Cam", Description = "Full HD dash cam with loop recording and night vision.", Price = 69.95m, Tags = new[] { "dashcam", "camera", "car", "security" } },
                new { Category = ProductCategory.Electronics, Name = "Smart Glasses with AR Display", Description = "Augmented reality glasses with heads-up display and smartphone integration.", Price = 249.99m, Tags = new[] { "ar", "glasses", "smart", "wearable" } },
                new { Category = ProductCategory.Electronics, Name = "Digital Smart Pen", Description = "Smart pen that digitizes handwriting and syncs notes to cloud apps.", Price = 79.90m, Tags = new[] { "smart", "pen", "notes", "digital" } },
                new { Category = ProductCategory.Electronics, Name = "Mini Desktop Air Purifier", Description = "Compact HEPA air purifier for desks and small rooms.", Price = 39.95m, Tags = new[] { "air", "purifier", "hepa", "desktop" } },
                new { Category = ProductCategory.Electronics, Name = "Smart Water Bottle", Description = "Hydration-tracking bottle with temperature display and reminders.", Price = 49.99m, Tags = new[] { "water", "smart", "hydration", "bottle" } },
                new { Category = ProductCategory.Electronics, Name = "Bluetooth Beanie Hat", Description = "Warm knit beanie with built-in Bluetooth speakers and microphone.", Price = 24.99m, Tags = new[] { "bluetooth", "hat", "beanie", "audio" } },
                new { Category = ProductCategory.Electronics, Name = "Smart Alarm Clock", Description = "Wake-up light alarm clock with sunrise simulation and white noise.", Price = 59.99m, Tags = new[] { "alarm", "smart", "clock", "light" } },
                new { Category = ProductCategory.Electronics, Name = "Digital Tape Measure", Description = "Electronic measuring tape with digital readout and memory function.", Price = 34.50m, Tags = new[] { "measure", "tool", "digital", "tape" } },
                new { Category = ProductCategory.Electronics, Name = "Electric Heated Blanket", Description = "Soft microplush blanket with adjustable heat settings and auto shut-off.", Price = 79.95m, Tags = new[] { "heated", "blanket", "electric", "cozy" } },
                new { Category = ProductCategory.Electronics, Name = "Smart Pet Feeder", Description = "WiFi-enabled automatic pet feeder with portion control and camera.", Price = 119.99m, Tags = new[] { "pet", "feeder", "smart", "wifi" } },
                new { Category = ProductCategory.Electronics, Name = "Digital Luggage Scale", Description = "Portable digital scale for luggage with backlit display and tare function.", Price = 14.99m, Tags = new[] { "scale", "luggage", "travel", "digital" } },
                new { Category = ProductCategory.Electronics, Name = "Portable Air Compressor", Description = "Cordless air pump for car tires and sports equipment with LCD display.", Price = 59.00m, Tags = new[] { "compressor", "air", "portable", "pump" } },
                new { Category = ProductCategory.Electronics, Name = "Smart Bike Light Set", Description = "Rechargeable front and rear bike lights with auto-brightness and turn signals.", Price = 45.00m, Tags = new[] { "bike", "light", "smart", "safety" } },
                new { Category = ProductCategory.Electronics, Name = "Electric Wine Opener Set", Description = "Rechargeable electric wine opener with foil cutter and aerator.", Price = 36.99m, Tags = new[] { "wine", "opener", "electric", "kitchen" } },
                new { Category = ProductCategory.Electronics, Name = "Smart Meat Thermometer", Description = "Wireless food thermometer with app alerts and real-time tracking.", Price = 69.95m, Tags = new[] { "thermometer", "kitchen", "wireless", "bbq" } },
                new { Category = ProductCategory.Electronics, Name = "Digital Night Vision Goggles", Description = "Infrared night vision with photo and video recording.", Price = 189.99m, Tags = new[] { "night-vision", "goggles", "infrared", "digital" } },
                new { Category = ProductCategory.Electronics, Name = "Electronic Drum Pad Set", Description = "Compact digital drum pad with multiple sound kits and headphone output.", Price = 99.00m, Tags = new[] { "drum", "pad", "music", "electronic" } },
                new { Category = ProductCategory.Electronics, Name = "Laser Engraving Machine", Description = "Desktop laser engraver for wood, leather, and acrylic.", Price = 249.50m, Tags = new[] { "engraving", "laser", "machine", "craft" } },
                new { Category = ProductCategory.Electronics, Name = "Smart Fingerprint Padlock", Description = "Biometric fingerprint padlock with USB recharge and no keys needed.", Price = 39.00m, Tags = new[] { "padlock", "fingerprint", "biometric", "security" } },
                new { Category = ProductCategory.Electronics, Name = "Electric Coffee Grinder", Description = "High-speed coffee grinder with adjustable coarseness settings.", Price = 54.99m, Tags = new[] { "coffee", "grinder", "electric", "kitchen" } },
                new { Category = ProductCategory.Electronics, Name = "Digital Microscope with Screen", Description = "Portable digital microscope with built-in LCD screen and USB output.", Price = 89.00m, Tags = new[] { "microscope", "digital", "science", "lcd" } },
                new { Category = ProductCategory.Electronics, Name = "Smart Leak Detector", Description = "WiFi water leak sensor with phone alerts and battery backup.", Price = 44.95m, Tags = new[] { "leak", "detector", "smart", "sensor" } },
                new { Category = ProductCategory.Electronics, Name = "Wireless Lavalier Microphone", Description = "Clip-on wireless mic system for video recording and streaming.", Price = 79.90m, Tags = new[] { "microphone", "wireless", "lavalier", "audio" } },
                new { Category = ProductCategory.Electronics, Name = "Rechargeable Motion Sensor Light", Description = "Magnetic LED light bar with motion sensor for closets and cabinets.", Price = 22.99m, Tags = new[] { "light", "motion", "sensor", "led" } },
                new { Category = ProductCategory.Electronics, Name = "USB Rechargeable Bike Horn", Description = "Loud electric bike horn with multiple sound modes and USB charging.", Price = 18.50m, Tags = new[] { "bike", "horn", "usb", "loud" } },
                new { Category = ProductCategory.Electronics, Name = "Desktop UV Sterilizer Box", Description = "UV-C light sterilization box for phones, keys, and accessories.", Price = 39.95m, Tags = new[] { "uv", "sterilizer", "box", "clean" } },
                new { Category = ProductCategory.Electronics, Name = "USB-Powered Mini Fridge", Description = "Tiny desktop fridge for drinks or snacks powered by USB.", Price = 34.99m, Tags = new[] { "fridge", "usb", "mini", "cooler" } },
                new { Category = ProductCategory.Electronics, Name = "LED Message Board", Description = "Customizable LED board with Bluetooth app for scrolling messages.", Price = 29.90m, Tags = new[] { "led", "message", "board", "display" } },
                new { Category = ProductCategory.Electronics, Name = "Electric Callus Remover", Description = "Rechargeable foot care tool with adjustable speed and rollers.", Price = 27.95m, Tags = new[] { "callus", "remover", "electric", "foot" } },
                new { Category = ProductCategory.Electronics, Name = "Smart Soil Sensor", Description = "Bluetooth soil monitor for plant moisture, light, and temperature.", Price = 34.95m, Tags = new[] { "soil", "sensor", "plant", "smart" } },
                new { Category = ProductCategory.Electronics, Name = "Wireless HDMI Transmitter", Description = "Stream HDMI content wirelessly from laptop or console to TV.", Price = 109.00m, Tags = new[] { "hdmi", "wireless", "streaming", "tv" } }
            };

            var clothingProducts = new[]
            {
                new { Category = ProductCategory.Clothing, Name = "Men's Classic White T-Shirt", Description = "100% cotton breathable white t-shirt with regular fit.", Price = 12.99m, Tags = new[] { "men", "tshirt", "cotton", "casual" } },
                new { Category = ProductCategory.Clothing, Name = "Women's Slim Fit Jeans", Description = "Stretch denim slim fit jeans with mid-rise waist.", Price = 39.95m, Tags = new[] { "women", "jeans", "denim", "slim-fit" } },
                new { Category = ProductCategory.Clothing, Name = "Unisex Hoodie with Kangaroo Pocket", Description = "Soft fleece hoodie with adjustable drawstrings and front pocket.", Price = 29.99m, Tags = new[] { "hoodie", "unisex", "fleece", "casual" } },
                new { Category = ProductCategory.Clothing, Name = "Men's Waterproof Windbreaker", Description = "Lightweight windbreaker jacket with waterproof coating.", Price = 49.99m, Tags = new[] { "men", "jacket", "waterproof", "windbreaker" } },
                new { Category = ProductCategory.Clothing, Name = "Women's Floral Summer Dress", Description = "Sleeveless A-line dress with floral print, perfect for summer.", Price = 34.95m, Tags = new[] { "women", "dress", "summer", "floral" } },
                new { Category = ProductCategory.Clothing, Name = "Men's Leather Belt", Description = "Genuine leather belt with classic metal buckle.", Price = 19.99m, Tags = new[] { "men", "belt", "leather", "accessory" } },
                new { Category = ProductCategory.Clothing, Name = "Women's Cotton Socks (5 Pack)", Description = "Soft cotton crew socks with reinforced toes and heels.", Price = 9.99m, Tags = new[] { "women", "socks", "cotton", "pack" } },
                new { Category = ProductCategory.Clothing, Name = "Men's Chino Pants", Description = "Slim fit chino pants made with stretch cotton blend.", Price = 34.50m, Tags = new[] { "men", "pants", "chino", "casual" } },
                new { Category = ProductCategory.Clothing, Name = "Women's Wool Blend Coat", Description = "Long wool blend coat with button closure and lapel collar.", Price = 129.99m, Tags = new[] { "women", "coat", "wool", "winter" } },
                new { Category = ProductCategory.Clothing, Name = "Unisex Sports Socks", Description = "Breathable sports socks with cushioned soles.", Price = 6.99m, Tags = new[] { "sports", "socks", "unisex", "athletic" } },
                new { Category = ProductCategory.Clothing, Name = "Men's Formal Dress Shirt", Description = "Button-down long sleeve dress shirt in solid colors.", Price = 24.99m, Tags = new[] { "men", "shirt", "formal", "dress" } },
                new { Category = ProductCategory.Clothing, Name = "Women's Yoga Pants", Description = "High-waisted stretchy yoga pants with moisture-wicking fabric.", Price = 27.50m, Tags = new[] { "women", "pants", "yoga", "stretch" } },
                new { Category = ProductCategory.Clothing, Name = "Men's Graphic T-Shirt", Description = "Cotton t-shirt with printed graphic design.", Price = 15.99m, Tags = new[] { "men", "tshirt", "graphic", "cotton" } },
                new { Category = ProductCategory.Clothing, Name = "Women's Denim Jacket", Description = "Classic denim jacket with button front and chest pockets.", Price = 44.99m, Tags = new[] { "women", "jacket", "denim", "casual" } },
                new { Category = ProductCategory.Clothing, Name = "Men's Athletic Shorts", Description = "Lightweight breathable shorts with elastic waistband.", Price = 18.95m, Tags = new[] { "men", "shorts", "athletic", "sportswear" } },
                new { Category = ProductCategory.Clothing, Name = "Women's Cardigan Sweater", Description = "Soft knit cardigan with button closure and pockets.", Price = 32.99m, Tags = new[] { "women", "sweater", "cardigan", "knit" } },
                new { Category = ProductCategory.Clothing, Name = "Unisex Baseball Cap", Description = "Adjustable baseball cap with embroidered logo.", Price = 14.50m, Tags = new[] { "hat", "cap", "unisex", "baseball" } },
                new { Category = ProductCategory.Clothing, Name = "Men's Wool Beanie", Description = "Warm wool beanie perfect for cold weather.", Price = 17.99m, Tags = new[] { "men", "beanie", "wool", "winter" } },
                new { Category = ProductCategory.Clothing, Name = "Women's Lace Blouse", Description = "Elegant lace blouse with scalloped edges and button-up front.", Price = 39.95m, Tags = new[] { "women", "blouse", "lace", "formal" } },
                new { Category = ProductCategory.Clothing, Name = "Men's Cargo Pants", Description = "Durable cargo pants with multiple pockets and relaxed fit.", Price = 29.99m, Tags = new[] { "men", "pants", "cargo", "outdoor" } },
                new { Category = ProductCategory.Clothing, Name = "Women's Leather Handbag", Description = "Stylish leather handbag with adjustable shoulder strap.", Price = 69.95m, Tags = new[] { "women", "handbag", "leather", "accessory" } },
                new { Category = ProductCategory.Clothing, Name = "Men's Thermal Underwear Set", Description = "Warm thermal underwear top and bottom set for winter.", Price = 22.50m, Tags = new[] { "men", "underwear", "thermal", "winter" } },
                new { Category = ProductCategory.Clothing, Name = "Women's Denim Skirt", Description = "Casual denim skirt with button-front design.", Price = 24.99m, Tags = new[] { "women", "skirt", "denim", "casual" } },
                new { Category = ProductCategory.Clothing, Name = "Unisex Rain Jacket", Description = "Lightweight waterproof rain jacket with hood.", Price = 39.99m, Tags = new[] { "rain", "jacket", "unisex", "waterproof" } },
                new { Category = ProductCategory.Clothing, Name = "Men's Oxford Shoes", Description = "Classic leather oxford dress shoes for formal occasions.", Price = 59.99m, Tags = new[] { "men", "shoes", "oxford", "formal" } },
                new { Category = ProductCategory.Clothing, Name = "Women's Running Shoes", Description = "Lightweight running shoes with breathable mesh upper.", Price = 54.99m, Tags = new[] { "women", "shoes", "running", "athletic" } },
                new { Category = ProductCategory.Clothing, Name = "Men's Hoodie Sweatshirt", Description = "Comfortable hoodie sweatshirt with front pocket and drawstring.", Price = 29.99m, Tags = new[] { "men", "hoodie", "sweatshirt", "casual" } },
                new { Category = ProductCategory.Clothing, Name = "Women's Scarf", Description = "Soft knit scarf suitable for all seasons.", Price = 14.95m, Tags = new[] { "women", "scarf", "knit", "accessory" } },
                new { Category = ProductCategory.Clothing, Name = "Men's Swim Trunks", Description = "Quick-dry swim trunks with elastic waistband and pockets.", Price = 19.95m, Tags = new[] { "men", "swimwear", "trunks", "summer" } },
                new { Category = ProductCategory.Clothing, Name = "Women's Blazer Jacket", Description = "Tailored blazer jacket suitable for office and casual wear.", Price = 49.99m, Tags = new[] { "women", "blazer", "jacket", "formal" } },
                new { Category = ProductCategory.Clothing, Name = "Men's Vintage Washed Tee", Description = "Pre-washed vintage look t-shirt made from organic cotton.", Price = 16.99m, Tags = new[] { "men", "tshirt", "vintage", "organic" } },
                new { Category = ProductCategory.Clothing, Name = "Women's High-Waisted Leggings", Description = "Seamless high-rise leggings perfect for workouts or lounging.", Price = 29.90m, Tags = new[] { "women", "leggings", "high-waist", "stretch" } },
                new { Category = ProductCategory.Clothing, Name = "Unisex Zip-Up Fleece Jacket", Description = "Full-zip fleece jacket with side pockets and breathable fabric.", Price = 39.99m, Tags = new[] { "jacket", "unisex", "fleece", "zip" } },
                new { Category = ProductCategory.Clothing, Name = "Men's Quilted Vest", Description = "Lightweight insulated vest with zip closure and stand collar.", Price = 44.50m, Tags = new[] { "men", "vest", "quilted", "outerwear" } },
                new { Category = ProductCategory.Clothing, Name = "Women's Ruffle Hem Skirt", Description = "Flowy skirt with tiered ruffles and elastic waistband.", Price = 27.95m, Tags = new[] { "women", "skirt", "ruffle", "summer" } },
                new { Category = ProductCategory.Clothing, Name = "Men's Reversible Leather Belt", Description = "Two-tone reversible belt crafted from genuine leather.", Price = 24.50m, Tags = new[] { "men", "belt", "reversible", "leather" } },
                new { Category = ProductCategory.Clothing, Name = "Women's Cozy Lounge Shorts", Description = "Soft knit shorts designed for maximum comfort at home.", Price = 13.99m, Tags = new[] { "women", "shorts", "lounge", "cozy" } },
                new { Category = ProductCategory.Clothing, Name = "Men's Stretch Cargo Shorts", Description = "Comfortable cargo shorts with stretch fabric and flap pockets.", Price = 31.50m, Tags = new[] { "men", "shorts", "cargo", "summer" } },
                new { Category = ProductCategory.Clothing, Name = "Women's Faux Fur Coat", Description = "Luxurious faux fur coat with satin lining and hook closure.", Price = 99.00m, Tags = new[] { "women", "coat", "fur", "winter" } },
                new { Category = ProductCategory.Clothing, Name = "Unisex Compression Socks", Description = "Supportive compression socks designed for all-day wear.", Price = 11.99m, Tags = new[] { "socks", "compression", "unisex", "support" } },
                new { Category = ProductCategory.Clothing, Name = "Men's Slim Fit Polo Shirt", Description = "Classic polo shirt with modern slim fit and soft cotton fabric.", Price = 22.99m, Tags = new[] { "men", "polo", "shirt", "slim-fit" } },
                new { Category = ProductCategory.Clothing, Name = "Women's Workout Tank Top", Description = "Lightweight tank top with racerback design and mesh panels.", Price = 18.50m, Tags = new[] { "women", "tank", "workout", "fitness" } },
                new { Category = ProductCategory.Clothing, Name = "Men's Long Sleeve Henley", Description = "Casual henley with 3-button placket and ribbed cuffs.", Price = 21.00m, Tags = new[] { "men", "henley", "long-sleeve", "casual" } },
                new { Category = ProductCategory.Clothing, Name = "Women's Oversized Denim Shirt", Description = "Relaxed-fit denim shirt with button closure and rolled sleeves.", Price = 38.95m, Tags = new[] { "women", "shirt", "denim", "oversized" } },
                new { Category = ProductCategory.Clothing, Name = "Men's Quick-Dry Running Shorts", Description = "Breathable running shorts with built-in liner and reflective trim.", Price = 26.99m, Tags = new[] { "men", "shorts", "running", "quick-dry" } },
                new { Category = ProductCategory.Clothing, Name = "Women's Lightweight Knit Sweater", Description = "Crew neck sweater made with a breathable cotton-blend knit.", Price = 29.99m, Tags = new[] { "women", "sweater", "lightweight", "knit" } },
                new { Category = ProductCategory.Clothing, Name = "Unisex Trucker Hat", Description = "Classic mesh trucker cap with adjustable snapback closure.", Price = 16.50m, Tags = new[] { "hat", "cap", "unisex", "trucker" } },
                new { Category = ProductCategory.Clothing, Name = "Men's Ribbed Knit Scarf", Description = "Thick ribbed scarf for warmth and comfort in cold weather.", Price = 19.99m, Tags = new[] { "men", "scarf", "knit", "winter" } },
                new { Category = ProductCategory.Clothing, Name = "Women's Off-Shoulder Blouse", Description = "Flattering blouse with off-shoulder neckline and flared sleeves.", Price = 33.95m, Tags = new[] { "women", "blouse", "off-shoulder", "chic" } },
                new { Category = ProductCategory.Clothing, Name = "Men's Utility Joggers", Description = "Modern joggers with cargo pockets and elastic cuffs.", Price = 34.99m, Tags = new[] { "men", "joggers", "utility", "casual" } },
                new { Category = ProductCategory.Clothing, Name = "Women's Minimalist Crossbody Bag", Description = "Compact crossbody bag with magnetic flap and adjustable strap.", Price = 59.00m, Tags = new[] { "women", "bag", "crossbody", "minimalist" } },
                new { Category = ProductCategory.Clothing, Name = "Men's Insulated Base Layer", Description = "Thermal base layer set ideal for outdoor winter activities.", Price = 28.99m, Tags = new[] { "men", "base-layer", "thermal", "outdoor" } },
                new { Category = ProductCategory.Clothing, Name = "Women's Pleated Midi Skirt", Description = "Elegant pleated skirt with satin finish and side zipper.", Price = 37.99m, Tags = new[] { "women", "skirt", "pleated", "midi" } },
                new { Category = ProductCategory.Clothing, Name = "Unisex Packable Rain Poncho", Description = "Light poncho with hood that packs into its own pouch.", Price = 21.75m, Tags = new[] { "rain", "poncho", "unisex", "packable" } },
                new { Category = ProductCategory.Clothing, Name = "Men's Dress Loafers", Description = "Slip-on leather loafers with cushioned insole for comfort.", Price = 64.99m, Tags = new[] { "men", "shoes", "loafers", "formal" } },
                new { Category = ProductCategory.Clothing, Name = "Women's Knit Running Shoes", Description = "Breathable lightweight runners with memory foam insoles.", Price = 48.99m, Tags = new[] { "women", "shoes", "running", "knit" } },
                new { Category = ProductCategory.Clothing, Name = "Men's Crewneck Sweatshirt", Description = "Midweight fleece sweatshirt with classic crewneck cut.", Price = 27.95m, Tags = new[] { "men", "sweatshirt", "crewneck", "casual" } },
                new { Category = ProductCategory.Clothing, Name = "Women's Woven Shawl Wrap", Description = "Fringed woven wrap ideal for layering in transitional weather.", Price = 19.99m, Tags = new[] { "women", "shawl", "wrap", "layer" } },
                new { Category = ProductCategory.Clothing, Name = "Men's Tropical Print Swim Shorts", Description = "Bold print swim shorts with mesh lining and drawstring.", Price = 22.90m, Tags = new[] { "men", "swimwear", "shorts", "tropical" } },
                new { Category = ProductCategory.Clothing, Name = "Women's Cropped Blazer", Description = "Structured cropped blazer perfect for modern office wear.", Price = 52.00m, Tags = new[] { "women", "blazer", "cropped", "formal" } }
            };

            var booksProducts = new[]
            {
                new { Category = ProductCategory.Books, Name = "The Great Gatsby", Description = "Classic novel by F. Scott Fitzgerald set in the Jazz Age.", Price = 8.99m, Tags = new[] { "classic", "novel", "literature", "fiction" } },
                new { Category = ProductCategory.Books, Name = "1984", Description = "Dystopian novel by George Orwell about totalitarianism and surveillance.", Price = 7.99m, Tags = new[] { "dystopian", "fiction", "classic", "political" } },
                new { Category = ProductCategory.Books, Name = "To Kill a Mockingbird", Description = "Pulitzer Prize-winning novel by Harper Lee about racial injustice.", Price = 9.50m, Tags = new[] { "classic", "fiction", "racism", "law" } },
                new { Category = ProductCategory.Books, Name = "The Catcher in the Rye", Description = "Coming-of-age novel by J.D. Salinger.", Price = 6.99m, Tags = new[] { "classic", "fiction", "coming-of-age", "literature" } },
                new { Category = ProductCategory.Books, Name = "Sapiens: A Brief History of Humankind", Description = "Non-fiction book by Yuval Noah Harari exploring human evolution and history.", Price = 14.99m, Tags = new[] { "non-fiction", "history", "science", "anthropology" } },
                new { Category = ProductCategory.Books, Name = "The Hobbit", Description = "Fantasy novel by J.R.R. Tolkien, prelude to The Lord of the Rings.", Price = 10.99m, Tags = new[] { "fantasy", "adventure", "classic", "middle-earth" } },
                new { Category = ProductCategory.Books, Name = "Harry Potter and the Sorcerer's Stone", Description = "First book in the Harry Potter series by J.K. Rowling.", Price = 8.95m, Tags = new[] { "fantasy", "young-adult", "magic", "adventure" } },
                new { Category = ProductCategory.Books, Name = "Becoming", Description = "Memoir by Michelle Obama detailing her life and career.", Price = 12.99m, Tags = new[] { "memoir", "biography", "inspirational", "non-fiction" } },
                new { Category = ProductCategory.Books, Name = "Educated", Description = "Memoir by Tara Westover about growing up in a strict family and pursuing education.", Price = 13.50m, Tags = new[] { "memoir", "education", "non-fiction", "inspirational" } },
                new { Category = ProductCategory.Books, Name = "The Alchemist", Description = "Novel by Paulo Coelho about a shepherd’s journey to find his destiny.", Price = 9.25m, Tags = new[] { "fiction", "philosophy", "inspirational", "adventure" } },
                new { Category = ProductCategory.Books, Name = "The Da Vinci Code", Description = "Mystery thriller novel by Dan Brown involving secret societies and religious conspiracies.", Price = 7.99m, Tags = new[] { "thriller", "mystery", "fiction", "suspense" } },
                new { Category = ProductCategory.Books, Name = "Thinking, Fast and Slow", Description = "Psychology book by Daniel Kahneman exploring two systems of thought.", Price = 15.99m, Tags = new[] { "non-fiction", "psychology", "science", "cognitive" } },
                new { Category = ProductCategory.Books, Name = "Pride and Prejudice", Description = "Romantic novel by Jane Austen exploring manners and marriage in 19th century England.", Price = 6.50m, Tags = new[] { "classic", "romance", "literature", "fiction" } },
                new { Category = ProductCategory.Books, Name = "The Subtle Art of Not Giving a F*ck", Description = "Self-help book by Mark Manson about living a better life by caring less.", Price = 11.95m, Tags = new[] { "self-help", "psychology", "non-fiction", "motivational" } },
                new { Category = ProductCategory.Books, Name = "The Girl on the Train", Description = "Psychological thriller novel by Paula Hawkins.", Price = 8.99m, Tags = new[] { "thriller", "mystery", "fiction", "suspense" } },
                new { Category = ProductCategory.Books, Name = "The Road", Description = "Post-apocalyptic novel by Cormac McCarthy.", Price = 7.50m, Tags = new[] { "fiction", "post-apocalyptic", "drama", "literature" } },
                new { Category = ProductCategory.Books, Name = "Grit", Description = "Book by Angela Duckworth on the power of passion and perseverance.", Price = 13.99m, Tags = new[] { "psychology", "self-help", "non-fiction", "motivation" } },
                new { Category = ProductCategory.Books, Name = "The Silent Patient", Description = "Thriller novel by Alex Michaelides about a woman who stops speaking after a traumatic event.", Price = 9.99m, Tags = new[] { "thriller", "mystery", "fiction", "suspense" } },
                new { Category = ProductCategory.Books, Name = "Where the Crawdads Sing", Description = "Mystery novel by Delia Owens set in the marshes of North Carolina.", Price = 10.50m, Tags = new[] { "fiction", "mystery", "drama", "nature" } },
                new { Category = ProductCategory.Books, Name = "Atomic Habits", Description = "Book by James Clear about building good habits and breaking bad ones.", Price = 16.99m, Tags = new[] { "self-help", "non-fiction", "psychology", "habits" } },
                new { Category = ProductCategory.Books, Name = "The Power of Habit", Description = "Charles Duhigg's book on the science of habit formation.", Price = 14.50m, Tags = new[] { "non-fiction", "psychology", "self-help", "science" } },
                new { Category = ProductCategory.Books, Name = "Meditations", Description = "Philosophical work by Marcus Aurelius, Roman Emperor and Stoic thinker.", Price = 7.95m, Tags = new[] { "philosophy", "classic", "stoicism", "non-fiction" } },
                new { Category = ProductCategory.Books, Name = "The Art of War", Description = "Ancient Chinese military treatise attributed to Sun Tzu.", Price = 5.99m, Tags = new[] { "strategy", "military", "classic", "philosophy" } },
                new { Category = ProductCategory.Books, Name = "The Catch-22", Description = "Satirical novel by Joseph Heller about WWII airmen.", Price = 8.25m, Tags = new[] { "classic", "fiction", "satire", "war" } },
                new { Category = ProductCategory.Books, Name = "The Martian", Description = "Science fiction novel by Andy Weir about an astronaut stranded on Mars.", Price = 9.50m, Tags = new[] { "sci-fi", "fiction", "adventure", "space" } },
                new { Category = ProductCategory.Books, Name = "Dune", Description = "Epic science fiction novel by Frank Herbert.", Price = 11.99m, Tags = new[] { "sci-fi", "fiction", "classic", "adventure" } },
                new { Category = ProductCategory.Books, Name = "Educated Guess", Description = "Mystery novel with twists and suspense.", Price = 7.99m, Tags = new[] { "mystery", "thriller", "fiction", "suspense" } },
                new { Category = ProductCategory.Books, Name = "The Outsider", Description = "Horror and crime thriller by Stephen King.", Price = 10.99m, Tags = new[] { "horror", "thriller", "fiction", "crime" } },
                new { Category = ProductCategory.Books, Name = "Born a Crime", Description = "Memoir by Trevor Noah about growing up in apartheid South Africa.", Price = 12.50m, Tags = new[] { "memoir", "biography", "non-fiction", "humor" } },
                new { Category = ProductCategory.Books, Name = "The Handmaid's Tale", Description = "Dystopian novel by Margaret Atwood about a totalitarian regime.", Price = 8.75m, Tags = new[] { "dystopian", "fiction", "classic", "science-fiction" } },
                new { Category = ProductCategory.Books, Name = "Brave New World", Description = "Aldous Huxley's dystopian vision of a technologically controlled society.", Price = 7.95m, Tags = new[] { "dystopian", "classic", "fiction", "science-fiction" } },
                new { Category = ProductCategory.Books, Name = "The Midnight Library", Description = "A novel by Matt Haig about second chances and infinite lives.", Price = 9.99m, Tags = new[] { "fiction", "philosophy", "magical-realism", "life" } },
                new { Category = ProductCategory.Books, Name = "Norwegian Wood", Description = "A poignant love story by Haruki Murakami set in 1960s Tokyo.", Price = 8.45m, Tags = new[] { "romance", "fiction", "literature", "japan" } },
                new { Category = ProductCategory.Books, Name = "A Man Called Ove", Description = "Fredrik Backman’s heartwarming novel about a grumpy but loveable man.", Price = 10.25m, Tags = new[] { "fiction", "humor", "life", "drama" } },
                new { Category = ProductCategory.Books, Name = "Circe", Description = "Madeline Miller’s feminist retelling of the mythological sorceress.", Price = 11.50m, Tags = new[] { "mythology", "fantasy", "fiction", "retelling" } },
                new { Category = ProductCategory.Books, Name = "The Four Agreements", Description = "Don Miguel Ruiz’s practical guide to personal freedom.", Price = 8.75m, Tags = new[] { "self-help", "spirituality", "non-fiction", "philosophy" } },
                new { Category = ProductCategory.Books, Name = "Can’t Hurt Me", Description = "David Goggins’s memoir of mental toughness and perseverance.", Price = 13.99m, Tags = new[] { "memoir", "self-help", "non-fiction", "motivation" } },
                new { Category = ProductCategory.Books, Name = "Man’s Search for Meaning", Description = "Viktor Frankl’s account of surviving the Holocaust and finding purpose.", Price = 7.25m, Tags = new[] { "psychology", "memoir", "philosophy", "non-fiction" } },
                new { Category = ProductCategory.Books, Name = "The Book Thief", Description = "Markus Zusak’s novel set in Nazi Germany told through Death’s eyes.", Price = 9.99m, Tags = new[] { "historical", "fiction", "war", "literature" } },
                new { Category = ProductCategory.Books, Name = "All the Light We Cannot See", Description = "Anthony Doerr’s Pulitzer-winning novel of two teens during WWII.", Price = 11.99m, Tags = new[] { "historical", "fiction", "war", "literature" } },
                new { Category = ProductCategory.Books, Name = "The 5 AM Club", Description = "Robin Sharma’s method for maximizing productivity and mindset.", Price = 12.75m, Tags = new[] { "self-help", "productivity", "non-fiction", "motivation" } },
                new { Category = ProductCategory.Books, Name = "Outliers", Description = "Malcolm Gladwell explores what makes people successful.", Price = 14.25m, Tags = new[] { "non-fiction", "psychology", "success", "sociology" } },
                new { Category = ProductCategory.Books, Name = "Zero to One", Description = "Peter Thiel’s thoughts on startups and building the future.", Price = 10.50m, Tags = new[] { "business", "startups", "non-fiction", "technology" } },
                new { Category = ProductCategory.Books, Name = "Deep Work", Description = "Cal Newport’s guide to focused success in a distracted world.", Price = 13.50m, Tags = new[] { "productivity", "non-fiction", "self-help", "focus" } },
                new { Category = ProductCategory.Books, Name = "Quiet", Description = "Susan Cain celebrates the power of introverts.", Price = 12.00m, Tags = new[] { "psychology", "non-fiction", "introversion", "self-help" } },
                new { Category = ProductCategory.Books, Name = "The Body Keeps the Score", Description = "Bessel van der Kolk’s research on trauma and healing.", Price = 16.50m, Tags = new[] { "psychology", "trauma", "non-fiction", "science" } },
                new { Category = ProductCategory.Books, Name = "Daisy Jones & The Six", Description = "A rock-n-roll oral history-style novel by Taylor Jenkins Reid.", Price = 9.85m, Tags = new[] { "fiction", "music", "drama", "romance" } },
                new { Category = ProductCategory.Books, Name = "Malibu Rising", Description = "A beachside family drama by Taylor Jenkins Reid.", Price = 10.99m, Tags = new[] { "fiction", "drama", "family", "romance" } },
                new { Category = ProductCategory.Books, Name = "The Paris Library", Description = "Historical novel about a WWII librarian saving books in Nazi-occupied Paris.", Price = 8.95m, Tags = new[] { "historical", "fiction", "books", "war" } },
                new { Category = ProductCategory.Books, Name = "Before We Were Strangers", Description = "A love story about missed chances by Renée Carlino.", Price = 7.75m, Tags = new[] { "romance", "fiction", "love", "contemporary" } },
                new { Category = ProductCategory.Books, Name = "Verity", Description = "Colleen Hoover’s psychological thriller with twists and suspense.", Price = 9.45m, Tags = new[] { "thriller", "romance", "suspense", "fiction" } },
                new { Category = ProductCategory.Books, Name = "Reminders of Him", Description = "A mother rebuilding her life after prison, by Colleen Hoover.", Price = 8.25m, Tags = new[] { "romance", "fiction", "drama", "redemption" } },
                new { Category = ProductCategory.Books, Name = "Ugly Love", Description = "Intense emotional romance novel by Colleen Hoover.", Price = 7.95m, Tags = new[] { "romance", "fiction", "drama", "love" } },
                new { Category = ProductCategory.Books, Name = "The Seven Husbands of Evelyn Hugo", Description = "Hollywood secrets revealed in this glamorous novel.", Price = 11.25m, Tags = new[] { "fiction", "romance", "drama", "celebrity" } },
                new { Category = ProductCategory.Books, Name = "Project Hail Mary", Description = "Andy Weir’s sci-fi thriller about a lone astronaut saving Earth.", Price = 13.25m, Tags = new[] { "science-fiction", "space", "thriller", "fiction" } },
                new { Category = ProductCategory.Books, Name = "The House in the Cerulean Sea", Description = "Whimsical fantasy about magical children and found family.", Price = 9.99m, Tags = new[] { "fantasy", "fiction", "LGBTQ+", "magical-realism" } },
                new { Category = ProductCategory.Books, Name = "Klara and the Sun", Description = "Kazuo Ishiguro’s novel about AI and human emotion.", Price = 10.75m, Tags = new[] { "science-fiction", "AI", "literature", "fiction" } },
                new { Category = ProductCategory.Books, Name = "The Night Circus", Description = "A magical realism novel about a mysterious circus.", Price = 9.50m, Tags = new[] { "fantasy", "romance", "magic", "fiction" } },
                new { Category = ProductCategory.Books, Name = "The Paper Palace", Description = "Emotional novel about memory, family, and difficult choices.", Price = 8.95m, Tags = new[] { "fiction", "drama", "romance", "family" } },
                new { Category = ProductCategory.Books, Name = "Anxious People", Description = "A comedic and heartfelt hostage story by Fredrik Backman.", Price = 9.15m, Tags = new[] { "fiction", "humor", "drama", "psychology" } },
            };

            var toysProducts = new[]
            {
                new { Category = ProductCategory.Toys, Name = "Building Blocks Set", Description = "Colorful interlocking building blocks for creative play.", Price = 24.99m, Tags = new[] { "blocks", "building", "creative", "kids" } },
                new { Category = ProductCategory.Toys, Name = "Remote Control Car", Description = "Fast remote control car with rechargeable battery and LED lights.", Price = 39.99m, Tags = new[] { "remote-control", "car", "battery", "kids" } },
                new { Category = ProductCategory.Toys, Name = "Stuffed Teddy Bear", Description = "Soft and cuddly teddy bear suitable for all ages.", Price = 14.50m, Tags = new[] { "plush", "bear", "stuffed", "comfort" } },
                new { Category = ProductCategory.Toys, Name = "Puzzle Board Game", Description = "500-piece jigsaw puzzle with vibrant and colorful design.", Price = 19.99m, Tags = new[] { "puzzle", "board-game", "family", "strategy" } },
                new { Category = ProductCategory.Toys, Name = "Action Figure Set", Description = "Set of 5 collectible action figures with movable joints.", Price = 29.95m, Tags = new[] { "action-figure", "collectible", "toys", "kids" } },
                new { Category = ProductCategory.Toys, Name = "Kids Art & Craft Kit", Description = "Complete art and craft kit with paints, brushes, and papers.", Price = 22.00m, Tags = new[] { "art", "craft", "creative", "kids" } },
                new { Category = ProductCategory.Toys, Name = "Toy Train Set", Description = "Electric toy train set with tracks and remote control.", Price = 44.99m, Tags = new[] { "train", "electric", "toy", "kids" } },
                new { Category = ProductCategory.Toys, Name = "Educational Alphabet Blocks", Description = "Wooden alphabet blocks for early learning and play.", Price = 17.50m, Tags = new[] { "education", "blocks", "wooden", "learning" } },
                new { Category = ProductCategory.Toys, Name = "Dollhouse Furniture Set", Description = "Miniature furniture set for dollhouses with realistic details.", Price = 25.99m, Tags = new[] { "dollhouse", "furniture", "miniature", "toys" } },
                new { Category = ProductCategory.Toys, Name = "Outdoor Water Gun", Description = "High-capacity water gun for outdoor summer fun.", Price = 15.75m, Tags = new[] { "water-gun", "outdoor", "summer", "kids" } },
                new { Category = ProductCategory.Toys, Name = "Robot Building Kit", Description = "DIY robot kit with motors and sensors for STEM learning.", Price = 49.99m, Tags = new[] { "robot", "stem", "building", "education" } },
                new { Category = ProductCategory.Toys, Name = "Bubble Machine", Description = "Automatic bubble blower for outdoor parties and play.", Price = 18.99m, Tags = new[] { "bubble", "machine", "outdoor", "fun" } },
                new { Category = ProductCategory.Toys, Name = "Remote Control Helicopter", Description = "Lightweight RC helicopter with easy controls for beginners.", Price = 34.50m, Tags = new[] { "remote-control", "helicopter", "toy", "kids" } },
                new { Category = ProductCategory.Toys, Name = "Plush Dinosaur Toy", Description = "Soft plush dinosaur toy suitable for toddlers.", Price = 16.00m, Tags = new[] { "plush", "dinosaur", "stuffed", "kids" } },
                new { Category = ProductCategory.Toys, Name = "Mini Basketball Hoop", Description = "Adjustable mini basketball hoop for indoor and outdoor use.", Price = 27.99m, Tags = new[] { "basketball", "sports", "indoor", "outdoor" } },
                new { Category = ProductCategory.Toys, Name = "Kids Science Experiment Kit", Description = "Fun science experiments kit for children aged 8+.", Price = 35.00m, Tags = new[] { "science", "experiment", "education", "kids" } },
                new { Category = ProductCategory.Toys, Name = "Wooden Train Puzzle", Description = "Colorful wooden puzzle shaped like a train with large pieces.", Price = 20.00m, Tags = new[] { "wooden", "puzzle", "train", "kids" } },
                new { Category = ProductCategory.Toys, Name = "Toy Drum Set", Description = "Miniature drum set for toddlers with real drum sounds.", Price = 29.00m, Tags = new[] { "music", "drums", "toy", "kids" } },
                new { Category = ProductCategory.Toys, Name = "Magic Trick Set", Description = "Complete beginner’s magic set with props and instructions.", Price = 24.50m, Tags = new[] { "magic", "trick", "fun", "kids" } },
                new { Category = ProductCategory.Toys, Name = "Remote Control Boat", Description = "Waterproof RC boat with rechargeable battery and speed control.", Price = 42.00m, Tags = new[] { "remote-control", "boat", "waterproof", "toy" } },
                new { Category = ProductCategory.Toys, Name = "Stuffed Puppy Dog", Description = "Soft and cuddly plush puppy dog toy.", Price = 14.00m, Tags = new[] { "plush", "dog", "stuffed", "comfort" } },
                new { Category = ProductCategory.Toys, Name = "Kids Karaoke Microphone", Description = "Wireless karaoke mic with built-in speaker and fun sound effects.", Price = 33.99m, Tags = new[] { "karaoke", "microphone", "music", "kids" } },
                new { Category = ProductCategory.Toys, Name = "Stacking Rings Toy", Description = "Classic wooden stacking rings for toddlers to develop motor skills.", Price = 19.99m, Tags = new[] { "wooden", "stacking", "toddler", "education" } },
                new { Category = ProductCategory.Toys, Name = "Electric Race Car Track", Description = "Race car track set with electric cars and loop-the-loop.", Price = 55.00m, Tags = new[] { "race", "car", "track", "electric" } },
                new { Category = ProductCategory.Toys, Name = "Foam Dart Blaster", Description = "Safe foam dart gun with rapid-fire capability.", Price = 22.99m, Tags = new[] { "dart", "blaster", "toy", "fun" } },
                new { Category = ProductCategory.Toys, Name = "Interactive Storybook", Description = "Electronic storybook with sound and light features for kids.", Price = 28.50m, Tags = new[] { "storybook", "interactive", "education", "kids" } },
                new { Category = ProductCategory.Toys, Name = "Puzzle Cube", Description = "Classic 3x3 Rubik's Cube for brain-teasing fun.", Price = 12.99m, Tags = new[] { "puzzle", "cube", "brain", "toy" } },
                new { Category = ProductCategory.Toys, Name = "Ride-On Toy Car", Description = "Battery-operated ride-on car with realistic design and sounds.", Price = 120.00m, Tags = new[] { "ride-on", "car", "battery", "kids" } },
                new { Category = ProductCategory.Toys, Name = "Play-Doh Creative Set", Description = "Set of colorful Play-Doh tubs with molds and tools.", Price = 18.00m, Tags = new[] { "play-doh", "creative", "craft", "kids" } },
                new { Category = ProductCategory.Toys, Name = "Wooden Balance Board", Description = "Balance board toy for developing coordination and balance.", Price = 40.00m, Tags = new[] { "wooden", "balance", "toy", "kids" } },
                new { Category = ProductCategory.Toys, Name = "Glow in the Dark Stars", Description = "Set of adhesive glow-in-the-dark stars to decorate rooms.", Price = 9.99m, Tags = new[] { "glow", "stars", "decor", "kids" } },
                new { Category = ProductCategory.Toys, Name = "Mini Drone", Description = "Compact drone with easy controls and camera for beginners.", Price = 49.99m, Tags = new[] { "drone", "remote-control", "camera", "kids" } },
                new { Category = ProductCategory.Toys, Name = "Wooden Animal Puzzle", Description = "Eco-friendly wooden animal shaped puzzle for toddlers.", Price = 14.99m, Tags = new[] { "wooden", "puzzle", "animal", "education" } },
                new { Category = ProductCategory.Toys, Name = "Foam Building Blocks", Description = "Lightweight foam blocks for safe indoor construction play.", Price = 27.50m, Tags = new[] { "foam", "blocks", "building", "safe" } },
                new { Category = ProductCategory.Toys, Name = "Interactive Robot Pet", Description = "Electronic pet robot with sound and motion sensors.", Price = 59.99m, Tags = new[] { "robot", "interactive", "pet", "electronics" } },
                new { Category = ProductCategory.Toys, Name = "Kids Gardening Kit", Description = "Starter gardening set with tools and seeds for children.", Price = 22.00m, Tags = new[] { "gardening", "education", "outdoor", "kids" } },
                new { Category = ProductCategory.Toys, Name = "Magnetic Building Tiles", Description = "Colorful magnetic tiles for creative 3D construction.", Price = 34.99m, Tags = new[] { "magnetic", "tiles", "building", "creative" } },
                new { Category = ProductCategory.Toys, Name = "Watercolor Paint Set", Description = "Non-toxic watercolor paint kit with brushes and paper.", Price = 19.75m, Tags = new[] { "paint", "art", "creative", "kids" } },
                new { Category = ProductCategory.Toys, Name = "Toy Kitchen Set", Description = "Miniature kitchen with utensils and pretend food items.", Price = 45.00m, Tags = new[] { "kitchen", "pretend-play", "creative", "kids" } },
                new { Category = ProductCategory.Toys, Name = "Stackable Dinosaur Figures", Description = "Set of colorful stackable dinosaur toys for toddlers.", Price = 16.50m, Tags = new[] { "dinosaur", "stacking", "toy", "kids" } },
                new { Category = ProductCategory.Toys, Name = "Glow Stick Bracelets", Description = "Pack of glow-in-the-dark bracelets for party fun.", Price = 8.00m, Tags = new[] { "glow", "bracelets", "party", "fun" } },
                new { Category = ProductCategory.Toys, Name = "Solar Powered Car Kit", Description = "Build-your-own car powered by solar energy.", Price = 29.99m, Tags = new[] { "solar", "science", "building", "education" } },
                new { Category = ProductCategory.Toys, Name = "Kids Walkie Talkies", Description = "Pair of walkie talkies with long range for outdoor play.", Price = 24.99m, Tags = new[] { "walkie-talkie", "communication", "outdoor", "kids" } },
                new { Category = ProductCategory.Toys, Name = "Foam Glider Plane", Description = "Lightweight foam plane glider for outdoor flying fun.", Price = 12.75m, Tags = new[] { "foam", "plane", "outdoor", "fun" } },
                new { Category = ProductCategory.Toys, Name = "Kids Drum Pad", Description = "Electronic drum pad with various sound effects.", Price = 38.50m, Tags = new[] { "music", "drum", "electronic", "toy" } },
                new { Category = ProductCategory.Toys, Name = "Animal Finger Puppets", Description = "Set of colorful animal finger puppets for storytelling.", Price = 13.99m, Tags = new[] { "puppet", "animal", "creative", "kids" } },
                new { Category = ProductCategory.Toys, Name = "Bubble Wand Set", Description = "Giant bubble wand with bubble solution for outdoor play.", Price = 15.00m, Tags = new[] { "bubble", "outdoor", "fun", "kids" } },
                new { Category = ProductCategory.Toys, Name = "Kinetic Sand Kit", Description = "Set of colored kinetic sand for sensory play.", Price = 21.99m, Tags = new[] { "sand", "sensory", "creative", "kids" } },
                new { Category = ProductCategory.Toys, Name = "Mini Soccer Ball", Description = "Lightweight mini soccer ball for indoor and outdoor use.", Price = 14.00m, Tags = new[] { "sports", "soccer", "ball", "kids" } },
                new { Category = ProductCategory.Toys, Name = "LED Light-Up Yo-Yo", Description = "Yo-yo with colorful LED lights for evening play.", Price = 18.50m, Tags = new[] { "yo-yo", "led", "toy", "fun" } },
                new { Category = ProductCategory.Toys, Name = "Stacking Cups", Description = "Brightly colored stacking cups to develop motor skills.", Price = 10.99m, Tags = new[] { "stacking", "cups", "motor-skills", "kids" } },
                new { Category = ProductCategory.Toys, Name = "Play Tent", Description = "Pop-up play tent with fun designs for indoor/outdoor use.", Price = 32.99m, Tags = new[] { "tent", "play", "indoor", "outdoor" } },
                new { Category = ProductCategory.Toys, Name = "Coloring Book Bundle", Description = "Bundle of themed coloring books with crayons included.", Price = 14.50m, Tags = new[] { "coloring", "books", "art", "kids" } },
                new { Category = ProductCategory.Toys, Name = "Mini Puzzle Box", Description = "Compact puzzle box with multiple brain teasers.", Price = 16.75m, Tags = new[] { "puzzle", "brain-teaser", "toy", "education" } },
                new { Category = ProductCategory.Toys, Name = "Balance Beam", Description = "Portable foam balance beam to improve coordination.", Price = 27.00m, Tags = new[] { "balance", "coordination", "foam", "kids" } },
                new { Category = ProductCategory.Toys, Name = "Marble Run Set", Description = "Build-your-own marble run with colorful plastic pieces.", Price = 42.50m, Tags = new[] { "marble", "run", "building", "creative" } },
                new { Category = ProductCategory.Toys, Name = "Kids Puzzle Mat", Description = "Soft foam mat with puzzle pieces for play and learning.", Price = 35.00m, Tags = new[] { "puzzle", "mat", "foam", "kids" } },
                new { Category = ProductCategory.Toys, Name = "Mini Golf Set", Description = "Indoor mini golf set with putter, balls, and holes.", Price = 39.99m, Tags = new[] { "mini-golf", "sports", "indoor", "fun" } },
                new { Category = ProductCategory.Toys, Name = "Glow Stick Necklace", Description = "Pack of glow stick necklaces for parties and fun.", Price = 7.50m, Tags = new[] { "glow", "necklace", "party", "kids" } },
                new { Category = ProductCategory.Toys, Name = "Stuffed Unicorn", Description = "Soft plush unicorn toy with rainbow mane.", Price = 18.00m, Tags = new[] { "plush", "unicorn", "stuffed", "fantasy" } }
            };

            var homeProducts = new[]
            {
                new { Category = ProductCategory.Home, Name = "Cotton Bath Towels Set", Description = "Set of 6 soft and absorbent 100% cotton bath towels.", Price = 49.99m, Tags = new[] { "bath", "towels", "cotton", "set" } },
                new { Category = ProductCategory.Home, Name = "Wooden Dining Chair", Description = "Classic solid wood dining chair with cushioned seat.", Price = 89.50m, Tags = new[] { "wooden", "chair", "dining", "furniture" } },
                new { Category = ProductCategory.Home, Name = "Ceramic Flower Vase", Description = "Hand-painted ceramic vase with floral design.", Price = 35.00m, Tags = new[] { "vase", "ceramic", "decor", "flowers" } },
                new { Category = ProductCategory.Home, Name = "Area Rug 5x7 Feet", Description = "Soft and durable area rug with modern geometric patterns.", Price = 120.00m, Tags = new[] { "rug", "area", "floor", "decor" } },
                new { Category = ProductCategory.Home, Name = "Linen Tablecloth", Description = "Natural linen tablecloth with elegant hemstitching.", Price = 42.99m, Tags = new[] { "tablecloth", "linen", "dining", "home" } },
                new { Category = ProductCategory.Home, Name = "Decorative Throw Pillows (Set of 4)", Description = "Colorful decorative pillows with removable covers.", Price = 55.00m, Tags = new[] { "pillows", "throw", "decor", "cushions" } },
                new { Category = ProductCategory.Home, Name = "Solid Wood Coffee Table", Description = "Rustic solid wood coffee table with storage shelf.", Price = 210.00m, Tags = new[] { "coffee-table", "wood", "furniture", "living-room" } },
                new { Category = ProductCategory.Home, Name = "Natural Soy Wax Candles", Description = "Set of 3 natural soy wax scented candles with wooden wicks.", Price = 27.99m, Tags = new[] { "candles", "soy-wax", "scented", "decor" } },
                new { Category = ProductCategory.Home, Name = "Cotton Kitchen Towels", Description = "Set of 5 durable and absorbent cotton kitchen towels.", Price = 18.50m, Tags = new[] { "kitchen", "towels", "cotton", "cleaning" } },
                new { Category = ProductCategory.Home, Name = "Wicker Laundry Basket", Description = "Durable wicker laundry basket with fabric liner.", Price = 40.00m, Tags = new[] { "laundry", "basket", "wicker", "storage" } },
                new { Category = ProductCategory.Home, Name = "Recycled Glass Drinking Glasses", Description = "Set of 6 eco-friendly recycled glass tumblers.", Price = 34.99m, Tags = new[] { "glassware", "drinking", "eco-friendly", "set" } },
                new { Category = ProductCategory.Home, Name = "Handwoven Cotton Throw Blanket", Description = "Cozy handwoven throw blanket with fringed edges.", Price = 65.00m, Tags = new[] { "throw", "blanket", "cotton", "handmade" } },
                new { Category = ProductCategory.Home, Name = "Bamboo Cutting Board Set", Description = "Set of 3 eco-friendly bamboo cutting boards.", Price = 29.99m, Tags = new[] { "cutting-board", "bamboo", "kitchen", "eco" } },
                new { Category = ProductCategory.Home, Name = "Rustic Wall Shelf", Description = "Floating rustic wood wall shelf with metal brackets.", Price = 55.50m, Tags = new[] { "wall-shelf", "wood", "rustic", "storage" } },
                new { Category = ProductCategory.Home, Name = "Ceramic Planter Pots (Set of 3)", Description = "Set of 3 modern ceramic planter pots with drainage holes.", Price = 48.00m, Tags = new[] { "planters", "ceramic", "plants", "set" } },
                new { Category = ProductCategory.Home, Name = "Cotton Shower Curtain", Description = "Machine washable cotton shower curtain with subtle pattern.", Price = 39.99m, Tags = new[] { "shower", "curtain", "cotton", "bathroom" } },
                new { Category = ProductCategory.Home, Name = "Jute Area Rug", Description = "Natural fiber jute rug with non-slip backing.", Price = 80.00m, Tags = new[] { "rug", "jute", "natural", "floor" } },
                new { Category = ProductCategory.Home, Name = "Handcrafted Wooden Serving Tray", Description = "Elegant wooden tray with handles for serving and decor.", Price = 34.50m, Tags = new[] { "tray", "wooden", "serving", "decor" } },
                new { Category = ProductCategory.Home, Name = "Wool Felt Storage Baskets", Description = "Set of 2 soft wool felt baskets for organizing items.", Price = 37.99m, Tags = new[] { "storage", "baskets", "felt", "organizing" } },
                new { Category = ProductCategory.Home, Name = "Stoneware Dinner Plates (Set of 4)", Description = "Durable stoneware plates with rustic glaze finish.", Price = 75.00m, Tags = new[] { "dinnerware", "plates", "stoneware", "set" } },
                new { Category = ProductCategory.Home, Name = "Cotton Napkin Set", Description = "Set of 6 machine washable cotton napkins in neutral colors.", Price = 24.99m, Tags = new[] { "napkins", "cotton", "table", "dining" } },
                new { Category = ProductCategory.Home, Name = "Rattan Hanging Basket", Description = "Decorative rattan hanging basket for plants or storage.", Price = 28.50m, Tags = new[] { "rattan", "hanging", "basket", "decor" } },
                new { Category = ProductCategory.Home, Name = "Bamboo Bath Mat", Description = "Water-resistant bamboo bath mat with anti-slip backing.", Price = 46.99m, Tags = new[] { "bath-mat", "bamboo", "bathroom", "natural" } },
                new { Category = ProductCategory.Home, Name = "Handmade Ceramic Coasters", Description = "Set of 6 unique handmade ceramic drink coasters.", Price = 29.00m, Tags = new[] { "coasters", "ceramic", "handmade", "table" } },
                new { Category = ProductCategory.Home, Name = "Cotton Rope Storage Baskets", Description = "Set of 3 cotton rope baskets for stylish organization.", Price = 33.50m, Tags = new[] { "storage", "baskets", "cotton", "organizing" } },
                new { Category = ProductCategory.Home, Name = "Natural Wood Cutting Board", Description = "Large natural wood cutting board with juice groove.", Price = 39.99m, Tags = new[] { "cutting-board", "wood", "kitchen", "natural" } },
                new { Category = ProductCategory.Home, Name = "Seagrass Woven Basket", Description = "Handwoven seagrass basket perfect for blankets or toys.", Price = 42.00m, Tags = new[] { "basket", "seagrass", "woven", "storage" } },
                new { Category = ProductCategory.Home, Name = "Cotton Canvas Storage Bin", Description = "Durable cotton canvas bin with handles for storage.", Price = 28.99m, Tags = new[] { "storage", "bin", "cotton", "organizing" } },
                new { Category = ProductCategory.Home, Name = "Handcrafted Wooden Candle Holder", Description = "Elegant wooden candle holder with natural finish.", Price = 21.99m, Tags = new[] { "candle-holder", "wooden", "decor", "handmade" } },
                new { Category = ProductCategory.Home, Name = "Wool Blend Throw Blanket", Description = "Warm wool blend throw blanket in neutral tones.", Price = 68.00m, Tags = new[] { "throw", "blanket", "wool", "cozy" } },
                new { Category = ProductCategory.Home, Name = "Stoneware Coffee Mug Set", Description = "Set of 4 rustic stoneware coffee mugs.", Price = 39.99m, Tags = new[] { "coffee-mug", "stoneware", "set", "kitchen" } },
                new { Category = ProductCategory.Home, Name = "Natural Fiber Door Mat", Description = "Durable natural fiber door mat with non-slip backing.", Price = 25.00m, Tags = new[] { "doormat", "natural-fiber", "entryway", "home" } },
                new { Category = ProductCategory.Home, Name = "Velvet Cushion Covers (Set of 4)", Description = "Luxurious velvet cushion covers with hidden zipper.", Price = 45.00m, Tags = new[] { "cushion", "velvet", "cover", "set" } },
                new { Category = ProductCategory.Home, Name = "Adjustable Floor Lamp", Description = "Modern adjustable floor lamp with warm LED lighting.", Price = 89.99m, Tags = new[] { "lamp", "floor", "led", "lighting" } },
                new { Category = ProductCategory.Home, Name = "Reclaimed Wood Wall Art", Description = "Rustic wall art made from reclaimed wood planks.", Price = 65.50m, Tags = new[] { "wall-art", "wood", "rustic", "decor" } },
                new { Category = ProductCategory.Home, Name = "Memory Foam Bath Mat", Description = "Soft and absorbent memory foam mat for bathrooms.", Price = 30.00m, Tags = new[] { "bath-mat", "memory-foam", "bathroom", "comfort" } },
                new { Category = ProductCategory.Home, Name = "Ceramic Soap Dispenser", Description = "Handcrafted ceramic soap dispenser with pump.", Price = 22.99m, Tags = new[] { "soap", "dispenser", "ceramic", "bathroom" } },
                new { Category = ProductCategory.Home, Name = "Bamboo Utensil Holder", Description = "Eco-friendly bamboo holder for kitchen utensils.", Price = 24.00m, Tags = new[] { "bamboo", "utensil-holder", "kitchen", "eco" } },
                new { Category = ProductCategory.Home, Name = "Macrame Plant Hanger", Description = "Hand-knotted cotton macrame hanger for indoor plants.", Price = 28.50m, Tags = new[] { "macrame", "plant", "hanger", "decor" } },
                new { Category = ProductCategory.Home, Name = "Faux Fur Throw Blanket", Description = "Soft and cozy faux fur blanket for living rooms.", Price = 75.00m, Tags = new[] { "throw", "blanket", "faux-fur", "cozy" } },
                new { Category = ProductCategory.Home, Name = "Iron Wine Rack", Description = "Compact iron wine rack holding up to 6 bottles.", Price = 40.99m, Tags = new[] { "wine-rack", "iron", "kitchen", "storage" } },
                new { Category = ProductCategory.Home, Name = "Natural Linen Curtains", Description = "Light-filtering natural linen curtains with rod pocket.", Price = 60.00m, Tags = new[] { "curtains", "linen", "natural", "window" } },
                new { Category = ProductCategory.Home, Name = "Concrete Planter", Description = "Modern minimalist concrete planter for succulents.", Price = 35.00m, Tags = new[] { "planter", "concrete", "succulent", "modern" } },
                new { Category = ProductCategory.Home, Name = "Wall-Mounted Coat Rack", Description = "Wood and metal coat rack with five hooks.", Price = 42.00m, Tags = new[] { "coat-rack", "wall-mounted", "wood", "metal" } },
                new { Category = ProductCategory.Home, Name = "Silk Pillowcase", Description = "Smooth silk pillowcase that protects hair and skin.", Price = 29.99m, Tags = new[] { "pillowcase", "silk", "bedroom", "luxury" } },
                new { Category = ProductCategory.Home, Name = "Rustic Candle Lantern", Description = "Decorative lantern with glass panels for candles.", Price = 38.50m, Tags = new[] { "lantern", "candle", "rustic", "decor" } },
                new { Category = ProductCategory.Home, Name = "Cotton Quilt Set", Description = "Lightweight cotton quilt with matching pillow shams.", Price = 120.00m, Tags = new[] { "quilt", "cotton", "bedroom", "set" } },
                new { Category = ProductCategory.Home, Name = "Ceramic Oil Diffuser", Description = "Ultrasonic diffuser with ceramic design for essential oils.", Price = 45.00m, Tags = new[] { "diffuser", "ceramic", "essential-oils", "aromatherapy" } },
                new { Category = ProductCategory.Home, Name = "Bamboo Bath Tray", Description = "Adjustable bamboo tray for baths with book holder.", Price = 49.99m, Tags = new[] { "bath-tray", "bamboo", "bathroom", "relaxation" } },
                new { Category = ProductCategory.Home, Name = "Leather Coasters Set", Description = "Set of 4 durable leather drink coasters.", Price = 27.00m, Tags = new[] { "coasters", "leather", "table", "drinkware" } },
                new { Category = ProductCategory.Home, Name = "Reversible Floor Pouf", Description = "Soft fabric pouf with reversible cover for seating.", Price = 55.00m, Tags = new[] { "pouf", "floor", "seating", "fabric" } },
                new { Category = ProductCategory.Home, Name = "Himalayan Salt Lamp", Description = "Natural Himalayan salt lamp with dimmable light.", Price = 35.99m, Tags = new[] { "lamp", "salt", "natural", "decor" } },
                new { Category = ProductCategory.Home, Name = "Bamboo Drawer Organizers", Description = "Set of bamboo dividers for drawer organization.", Price = 25.50m, Tags = new[] { "organizer", "bamboo", "drawer", "storage" } },
                new { Category = ProductCategory.Home, Name = "Cotton Waffle Weave Towels", Description = "Soft and absorbent waffle weave cotton towels.", Price = 38.00m, Tags = new[] { "towels", "cotton", "bathroom", "waffle" } },
                new { Category = ProductCategory.Home, Name = "Decorative Ceramic Bowl", Description = "Hand-painted ceramic bowl for serving or decor.", Price = 28.99m, Tags = new[] { "bowl", "ceramic", "decor", "handmade" } },
                new { Category = ProductCategory.Home, Name = "Natural Jute Storage Box", Description = "Eco-friendly jute box with lid for storage.", Price = 33.00m, Tags = new[] { "storage", "jute", "box", "eco" } },
                new { Category = ProductCategory.Home, Name = "Wool Dryer Balls (Set of 6)", Description = "Reusable wool balls that reduce drying time.", Price = 18.99m, Tags = new[] { "dryer-balls", "wool", "eco-friendly", "laundry" } },
                new { Category = ProductCategory.Home, Name = "Glass Butter Dish", Description = "Classic glass butter dish with lid.", Price = 15.99m, Tags = new[] { "butter-dish", "glass", "kitchen", "tableware" } },
                new { Category = ProductCategory.Home, Name = "Bamboo Salad Servers", Description = "Set of 2 bamboo salad serving utensils.", Price = 19.50m, Tags = new[] { "bamboo", "salad", "servers", "kitchen" } },
                new { Category = ProductCategory.Home, Name = "Faux Leather Ottoman", Description = "Compact faux leather ottoman with storage inside.", Price = 85.00m, Tags = new[] { "ottoman", "storage", "faux-leather", "furniture" } },
                new { Category = ProductCategory.Home, Name = "Cotton Quilted Oven Mitts", Description = "Heat-resistant quilted cotton oven mitts, set of 2.", Price = 16.99m, Tags = new[] { "oven-mitts", "cotton", "kitchen", "safety" } },
                new { Category = ProductCategory.Home, Name = "Reclaimed Wood Jewelry Box", Description = "Handmade jewelry box crafted from reclaimed wood.", Price = 50.00m, Tags = new[] { "jewelry-box", "wood", "reclaimed", "handmade" } }
            };

            var allProducts = electronicsProducts.ToList();
            allProducts.AddRange(clothingProducts);
            allProducts.AddRange(booksProducts);
            allProducts.AddRange(toysProducts);
            allProducts.AddRange(homeProducts);

            var usedProductIndexes = new HashSet<int>();

            List<Product> usedProducts = new List<Product>();

            foreach (AppUser user in users)
            {
                // Generate between 0 and 15 products

                int productCount = Rnd.Next(0, 16);

                for (int i = 0; i < productCount; i++)
                {
                    // Find a product that hasn't been used yet

                    int productIndex;

                    do
                    {
                        // NOTE: This will stall if it can't find one

                        productIndex = Rnd.Next(allProducts.Count);
                    } while (usedProductIndexes.Contains(productIndex));

                    usedProductIndexes.Add(productIndex);

                    var prodData = allProducts[productIndex];

                    // Add this product

                    var product = new Product
                    {
                        PublicId = await PublicIdGeneration.GenerateProductId(context),
                        Name = prodData.Name,
                        Description = prodData.Description,
                        Category = prodData.Category,
                        Price = prodData.Price,
                        Stock = Rnd.Next(0, 101), // Random stock 0-100
                        OwnerId = user.Id,
                        CreatedAt = DateTime.UtcNow,
                        IsSeeded = true
                    };

                    context.Products.Add(product);
                    await context.SaveChangesAsync(); // Save so product.Id is generated

                    usedProducts.Add(product);

                    // Add the product tags

                    foreach (var tagName in prodData.Tags)
                    {
                        var tag = await context.ProductTags.FirstOrDefaultAsync(t => t.Name == tagName);

                        if (tag == null)
                        {
                            // Tag doesn't exist - create it
                            tag = new ProductTag { Name = tagName, ProductId = product.Id };
                            context.ProductTags.Add(tag);
                        }
                    }

                    await context.SaveChangesAsync();
                }
            }

            System.Diagnostics.Debug.WriteLine($"Seeded all products");

            // Seed Orders

            var orderNotes = new List<string>
            {
                "Please leave the package by the back door.",
                "Gift – no price tag, please.",
                "Call upon arrival.",
                "Deliver between 10am and 2pm if possible.",
                "Leave with neighbour if I’m not home.",
                "This is a birthday present!",
                "Fragile item – handle with care.",
                "Do not bend the envelope.",
                "Wrap items separately, if possible.",
                "First-time customer – excited to try this!",
                "Please deliver to reception desk.",
                "Urgent – need by Friday.",
                "Let me know if anything is out of stock.",
                "Add a thank-you card, if available.",
                "I may not answer the door, just leave it outside.",
                "Use minimal packaging.",
                "Please don’t include a printed invoice.",
                "Eco-friendly packaging preferred.",
                "Use a different courier if available.",
                "I’d like to reorder this monthly.",
                "Order for an upcoming event.",
                "Please text before delivery.",
                "Great service last time, thank you!",
                "Avoid leaving in direct sunlight.",
                "Deliver to office, not home.",
                "Box should be discreet – no branding.",
                "Careful – pet may be loose in yard.",
                "This is for a school project.",
                "Please ensure all items are included.",
                "No substitutions, please.",
                "Gift order – please include gift receipt.",
                "Please confirm shipping time if delayed.",
                "Package arrived damaged last time – be cautious.",
                "Let me know when it ships.",
                "Please ship items together.",
                "Separate this from my subscription order.",
                "Use signature delivery.",
                "Happy to support local business!",
                "Will reorder next month if satisfied.",
                "Let me know if anything is backordered.",
                "No rush – standard shipping is fine.",
                "I'm allergic to peanuts – no samples, please.",
                "Looking forward to trying this!",
                "Great price – couldn’t pass this up.",
                "Mark as delivered only when actually delivered.",
                "Please don’t tape the box too tightly.",
                "Item is a surprise gift – be discreet.",
                "Please reuse packaging if possible.",
                "Leave in parcel locker if available.",
                "Thanks for the fast shipping last time!"
            };

            List<Order> usedOrders = new List<Order>();

            foreach (AppUser user in users)
            {
                // Decide on the amount of orders to create for this user (between 0 to 20)

                int orderAmount = Rnd.Next(0, 21);

                for (int i = 0; i < orderAmount; i++)
                {
                    // Get the product to order

                    Shuffle(usedProducts);

                    Product? product = usedProducts
                        .Where(p => p.OwnerId != user.Id)
                        .FirstOrDefault();

                    if (product is null)
                    {
                        continue;
                    }

                    // Get the order status

                    OrderStatus orderStatus = OrderStatus.Processing;

                    switch (Rnd.Next(1, 21))
                    {
                        case 1:
                        case 2: // 10% chance
                            orderStatus = OrderStatus.Pending;
                            break;
                        case 3:
                        case 4: // 10% chance
                            orderStatus = OrderStatus.Complete;
                            break;
                        case 5: // 5% chance
                            orderStatus = OrderStatus.Cancelled;
                            break;
                        default: // 75% chance
                            orderStatus = OrderStatus.Processing;
                            break;
                    }

                    // Get the quantity (between 1 and 3)

                    int quantity = Rnd.Next(1, 4);

                    // Get the order note to use

                    string orderNote = string.Empty;

                    if (Rnd.NextDouble() < 0.5) // 50% chance of using an order note
                    {
                        orderNote = orderNotes[Rnd.Next(0, orderNotes.Count)];
                    }

                    // Increment product sales

                    product.Sales += 1;

                    // Add the order

                    Order order = new Order
                    {
                        PublicId = await PublicIdGeneration.GenerateOrderId(context),
                        ProductId = product.Id,
                        CustomerId = user.Id,
                        Status = orderStatus,
                        Quantity = quantity,
                        Price = product.Price,
                        Notes = orderNote,
                        CreatedAt = GetRandomSeedDate()
                    };

                    order.CreatedAt = GetRandomSeedDate();

                    context.Orders.Add(order);
                    await context.SaveChangesAsync(); // So order.Id is set

                    usedOrders.Add(order);
                }
            }

            await context.SaveChangesAsync();
        }

        private static void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Rnd.Next(n + 1); // Random index from 0 to n
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private static DateTime GetRandomSeedDate()
        {
            // For seeding, use a Rnd date within the last 6 months

            DateTime now = DateTime.Now;
            DateTime sixMonthsAgo = now.AddMonths(-6);

            TimeSpan range = now - sixMonthsAgo;
            int totalSeconds = (int)range.TotalSeconds;

            int RndSeconds = Rnd.Next(0, totalSeconds);

            return now.AddSeconds(-RndSeconds);
        }
    }
}
