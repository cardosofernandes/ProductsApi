using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProductsService.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "id", "created_at", "name", "updated_at" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 11, 10, 21, 55, 53, 121, DateTimeKind.Utc).AddTicks(4670), "Electronics", null },
                    { 2, new DateTime(2026, 11, 10, 21, 55, 53, 122, DateTimeKind.Utc).AddTicks(3750), "Clothing", null },
                    { 3, new DateTime(2026, 11, 10, 21, 55, 53, 122, DateTimeKind.Utc).AddTicks(3850), "Home and Kitchen", null }
                });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "id", "category_id", "created_at", "description", "name", "price", "stock", "updated_at" },
                values: new object[,]
                {
                    { 100000, 1, new DateTime(2026, 11, 10, 21, 55, 53, 127, DateTimeKind.Utc).AddTicks(6510), "Ergonomic mouse with adjustable sensitivity.", "Wireless Mouse", 24.99m, 60, null },
                    { 100001, 1, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3410), "Portable battery with USB-A and USB-C outputs.", "Power Bank 10000mAh", 22.5m, 75, null },
                    { 100002, 1, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3450), "Compact solid-state drive with USB-C connectivity.", "Portable SSD 1TB", 89.99m, 20, null },
                    { 100003, 1, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3460), "Over-ear headphones with a rechargeable battery.", "Bluetooth Headphones", 69.99m, 45, null },
                    { 100004, 1, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3470), "Rechargeable speaker for indoor and outdoor use.", "Portable Bluetooth Speaker", 39.99m, 40, null },
                    { 100005, 3, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3480), "Electric kettle with a 1.7-litre capacity.", "Stainless Steel Kettle", 34.99m, 24, null },
                    { 100006, 2, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3490), "White button-down shirt in size L.", "Oxford Shirt White L", 34.99m, 25, null },
                    { 100007, 1, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3500), "Desktop microphone for calls and recordings.", "USB Microphone", 59.99m, 18, null },
                    { 100008, 2, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3510), "Brown leather belt in size M.", "Leather Belt Brown M", 22.99m, 20, null },
                    { 100009, 1, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3520), "Dimmable desk lamp with adjustable colour temperature.", "LED Desk Lamp", 27.99m, 33, null },
                    { 100010, 1, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3530), "Compact wall charger for compatible devices.", "USB-C Charger 65W", 29.99m, 65, null },
                    { 100011, 1, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3530), "Wi-Fi plug with scheduling support.", "Smart Plug", 14.99m, 48, null },
                    { 100012, 3, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3540), "Soft grey cotton bath towel.", "Bath Towel Grey", 16.99m, 65, null },
                    { 100013, 2, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3550), "Black zip-front hoodie in size L.", "Zip Hoodie Black L", 36.99m, 30, null },
                    { 100014, 2, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3560), "White cotton crew-neck T-shirt in size L.", "Cotton T-Shirt White L", 12.99m, 70, null },
                    { 100015, 2, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3570), "Lightweight green rain jacket in size L.", "Rain Jacket Green L", 59.99m, 15, null },
                    { 100016, 1, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3580), "USB webcam with a built-in microphone.", "Full HD Webcam", 34.99m, 30, null },
                    { 100017, 3, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3590), "Compact toaster with adjustable browning settings.", "Two-Slice Toaster", 29.99m, 19, null },
                    { 100018, 2, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3600), "Lightweight black sports shorts in size M.", "Running Shorts Black M", 19.99m, 45, null },
                    { 100019, 3, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3610), "Four ceramic mugs with a 300ml capacity each.", "Ceramic Mug Set of 4", 17.99m, 44, null },
                    { 100020, 1, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3620), "Padded sleeve for a ten-inch tablet.", "Tablet Sleeve", 18.99m, 0, null },
                    { 100021, 2, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3640), "Stretch sports leggings in size S.", "Sports Leggings Black S", 27.99m, 38, null },
                    { 100022, 3, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3640), "Four white ceramic dinner plates.", "Dinner Plate Set of 4", 27.99m, 28, null },
                    { 100023, 3, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3650), "Kitchen knife with a twenty-centimetre blade.", "Chef Knife 20cm", 19.99m, 35, null },
                    { 100024, 3, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3660), "Stainless steel cutlery set for four people.", "Cutlery Set 16 Pieces", 29.99m, 21, null },
                    { 100025, 2, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3670), "Black cotton crew-neck T-shirt in size M.", "Cotton T-Shirt Black M", 12.99m, 80, null },
                    { 100026, 2, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3680), "Five pairs of everyday cotton-blend socks.", "Cotton Socks Pack of 5", 9.99m, 120, null },
                    { 100027, 3, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3690), "Bamboo board for everyday food preparation.", "Bamboo Chopping Board", 14.99m, 50, null },
                    { 100028, 2, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3700), "Beige chinos with a 34-inch waist.", "Chino Trousers Beige 34", 39.99m, 22, null },
                    { 100029, 2, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3700), "Navy short-sleeve polo shirt in size M.", "Polo Shirt Navy M", 24.99m, 35, null },
                    { 100030, 3, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3710), "Twenty-eight-centimetre pan with a non-stick coating.", "Non-Stick Frying Pan 28cm", 25.99m, 32, null },
                    { 100031, 3, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3720), "Beige cushion cover with a concealed zip.", "Cushion Cover Beige", 9.99m, 58, null },
                    { 100032, 2, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3730), "Straight-leg blue jeans with a 32-inch waist.", "Denim Jeans Blue 32", 44.99m, 40, null },
                    { 100033, 2, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3740), "Navy cap with an adjustable fastening.", "Baseball Cap Navy", 14.99m, 42, null },
                    { 100034, 2, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3750), "Soft burgundy scarf for cold weather.", "Fleece Scarf Burgundy", 13.99m, 0, null },
                    { 100035, 1, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3760), "In-ear headphones with a charging case.", "Wireless Earbuds", 44.9m, 55, null },
                    { 100036, 3, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3770), "Three absorbent cotton kitchen towels.", "Cotton Tea Towels Pack of 3", 8.99m, 90, null },
                    { 100037, 2, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3770), "Navy knitted jumper in size M.", "Crew-Neck Jumper Navy M", 29.99m, 18, null },
                    { 100038, 2, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3780), "Grey knitted beanie with a folded cuff.", "Winter Beanie Grey", 11.5m, 55, null },
                    { 100039, 3, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3790), "Three food-storage containers with fitted lids.", "Glass Storage Containers Set", 24.99m, 40, null },
                    { 100040, 1, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3800), "Two-metre HDMI cable for displays and televisions.", "HDMI Cable 2m", 9.99m, 100, null },
                    { 100041, 3, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3860), "Compartment organiser for stationery and accessories.", "Desk Organiser", 12.99m, 46, null },
                    { 100042, 3, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3870), "Ventilated laundry basket with carry handles.", "Laundry Basket 40L", 18.5m, 23, null },
                    { 100043, 1, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3870), "Adjustable aluminium stand for laptops.", "Laptop Stand", 32.0m, 26, null },
                    { 100044, 1, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3880), "Multiport hub with HDMI and USB connections.", "USB-C Hub", 39.95m, 28, null },
                    { 100045, 3, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3890), "Two-litre stainless steel saucepan with a lid.", "Saucepan 2L", 22.99m, 27, null },
                    { 100046, 1, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3900), "Compact wireless keyboard with quiet keys.", "Wireless Keyboard", 49.99m, 35, null },
                    { 100047, 1, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3910), "Full HD monitor with an adjustable stand.", "27-inch Monitor", 159.99m, 12, null },
                    { 100048, 2, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3920), "Grey hooded sweatshirt in size M.", "Pullover Hoodie Grey M", 32.99m, 50, null },
                    { 100049, 3, new DateTime(2026, 11, 10, 21, 55, 53, 128, DateTimeKind.Utc).AddTicks(3930), "Soft fleece blanket for a sofa or bed.", "Fleece Throw Blanket", 21.99m, 37, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100000);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100001);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100002);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100003);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100004);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100005);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100006);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100007);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100008);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100009);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100010);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100011);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100012);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100013);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100014);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100015);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100016);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100017);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100018);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100019);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100020);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100021);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100022);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100023);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100024);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100025);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100026);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100027);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100028);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100029);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100030);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100031);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100032);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100033);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100034);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100035);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100036);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100037);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100038);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100039);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100040);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100041);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100042);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100043);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100044);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100045);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100046);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100047);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100048);

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: 100049);

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "id",
                keyValue: 3);
        }
    }
}
