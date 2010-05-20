
USE `npetshop`;

#
# Dumping data for table `accounts`
#
INSERT INTO `accounts` (Account_Id, Account_Email, Account_FirstName, Account_LastName, Account_Status, Account_Addr1, Account_Addr2, Account_City, Account_State, Account_Zip, Account_Country, Account_Phone) VALUES ('w', 'w', 'w', 'w', '', 'w', 'w', 'w', 'New York', 'w', 'Canada', 'w');

#
# Dumping data for table `categories`
#
INSERT INTO `categories` (Category_Id, Category_Name, Category_Description) VALUES ('BIRDS', 'Birds', NULL);
INSERT INTO `categories` (Category_Id, Category_Name, Category_Description) VALUES ('CATS', 'Cats', NULL);
INSERT INTO `categories` (Category_Id, Category_Name, Category_Description) VALUES ('DOGS', 'Dogs', NULL);
INSERT INTO `categories` (Category_Id, Category_Name, Category_Description) VALUES ('FISH', 'Fish', NULL);
INSERT INTO `categories` (Category_Id, Category_Name, Category_Description) VALUES ('REPTILES', 'Reptiles', NULL);

#
# Dumping data for table `inventories`
#
INSERT INTO `inventories` (Item_Id, Inventory_Quantity) VALUES ('EST-1', 10000);
INSERT INTO `inventories` (Item_Id, Inventory_Quantity) VALUES ('EST-2', 10000);
INSERT INTO `inventories` (Item_Id, Inventory_Quantity) VALUES ('EST-3', 555);
INSERT INTO `inventories` (Item_Id, Inventory_Quantity) VALUES ('EST-4', 9999);
INSERT INTO `inventories` (Item_Id, Inventory_Quantity) VALUES ('EST-5', 10000);
INSERT INTO `inventories` (Item_Id, Inventory_Quantity) VALUES ('EST-6', 10000);
INSERT INTO `inventories` (Item_Id, Inventory_Quantity) VALUES ('EST-7', 10000);
INSERT INTO `inventories` (Item_Id, Inventory_Quantity) VALUES ('EST-8', 10000);
INSERT INTO `inventories` (Item_Id, Inventory_Quantity) VALUES ('EST-9', 10000);
INSERT INTO `inventories` (Item_Id, Inventory_Quantity) VALUES ('EST-10', 10000);
INSERT INTO `inventories` (Item_Id, Inventory_Quantity) VALUES ('EST-11', 10000);
INSERT INTO `inventories` (Item_Id, Inventory_Quantity) VALUES ('EST-12', 10000);
INSERT INTO `inventories` (Item_Id, Inventory_Quantity) VALUES ('EST-13', 9999);
INSERT INTO `inventories` (Item_Id, Inventory_Quantity) VALUES ('EST-14', 10000);
INSERT INTO `inventories` (Item_Id, Inventory_Quantity) VALUES ('EST-15', 10000);
INSERT INTO `inventories` (Item_Id, Inventory_Quantity) VALUES ('EST-16', 10000);
INSERT INTO `inventories` (Item_Id, Inventory_Quantity) VALUES ('EST-17', 10000);
INSERT INTO `inventories` (Item_Id, Inventory_Quantity) VALUES ('EST-18', 10000);
INSERT INTO `inventories` (Item_Id, Inventory_Quantity) VALUES ('EST-19', 10000);
INSERT INTO `inventories` (Item_Id, Inventory_Quantity) VALUES ('EST-20', 10000);
INSERT INTO `inventories` (Item_Id, Inventory_Quantity) VALUES ('EST-21', 10000);
INSERT INTO `inventories` (Item_Id, Inventory_Quantity) VALUES ('EST-22', 10000);
INSERT INTO `inventories` (Item_Id, Inventory_Quantity) VALUES ('EST-23', 10000);
INSERT INTO `inventories` (Item_Id, Inventory_Quantity) VALUES ('EST-24', 10000);
INSERT INTO `inventories` (Item_Id, Inventory_Quantity) VALUES ('EST-25', 10000);
INSERT INTO `inventories` (Item_Id, Inventory_Quantity) VALUES ('EST-26', 10000);
INSERT INTO `inventories` (Item_Id, Inventory_Quantity) VALUES ('EST-27', 10000);
INSERT INTO `inventories` (Item_Id, Inventory_Quantity) VALUES ('EST-28', 10000);

#
# Dumping data for table `items`
#
INSERT INTO `items` (Item_Id, Product_Id, Item_ListPrice, Item_UnitCost, Supplier_Id, Item_Status, Item_Attr1, Item_Attr2, Item_Attr3, Item_Attr4, Item_Attr5, Item_Photo) VALUES ('EST-1', 'FI-SW-01', 16.50, 10.00, 1, 'P', 'Large', NULL, NULL, NULL, NULL,'fish1.jpg');
INSERT INTO `items` (Item_Id, Product_Id, Item_ListPrice, Item_UnitCost, Supplier_Id, Item_Status, Item_Attr1, Item_Attr2, Item_Attr3, Item_Attr4, Item_Attr5, Item_Photo) VALUES ('EST-2', 'FI-SW-01', 16.50, 10.00, 1, 'P', 'Small', NULL, NULL, NULL, NULL,'fish1.jpg');
INSERT INTO `items` (Item_Id, Product_Id, Item_ListPrice, Item_UnitCost, Supplier_Id, Item_Status, Item_Attr1, Item_Attr2, Item_Attr3, Item_Attr4, Item_Attr5, Item_Photo) VALUES ('EST-3', 'FI-SW-02', 18.50, 12.00, 1, 'P', 'Toothless', NULL, NULL, NULL, NULL,'fish2.jpg');
INSERT INTO `items` (Item_Id, Product_Id, Item_ListPrice, Item_UnitCost, Supplier_Id, Item_Status, Item_Attr1, Item_Attr2, Item_Attr3, Item_Attr4, Item_Attr5, Item_Photo) VALUES ('EST-4', 'FI-FW-01', 18.50, 12.00, 1, 'P', 'Spotted', NULL, NULL, NULL, NULL,'fish3.jpg');
INSERT INTO `items` (Item_Id, Product_Id, Item_ListPrice, Item_UnitCost, Supplier_Id, Item_Status, Item_Attr1, Item_Attr2, Item_Attr3, Item_Attr4, Item_Attr5, Item_Photo) VALUES ('EST-5', 'FI-FW-01', 18.50, 12.00, 1, 'P', 'Spotless', NULL, NULL, NULL, NULL,'fish3.jpg');
INSERT INTO `items` (Item_Id, Product_Id, Item_ListPrice, Item_UnitCost, Supplier_Id, Item_Status, Item_Attr1, Item_Attr2, Item_Attr3, Item_Attr4, Item_Attr5, Item_Photo) VALUES ('EST-6', 'K9-BD-01', 18.50, 12.00, 1, 'P', 'Male Adult', NULL, NULL, NULL, NULL,'dog1.jpg');
INSERT INTO `items` (Item_Id, Product_Id, Item_ListPrice, Item_UnitCost, Supplier_Id, Item_Status, Item_Attr1, Item_Attr2, Item_Attr3, Item_Attr4, Item_Attr5, Item_Photo) VALUES ('EST-7', 'K9-BD-01', 18.50, 12.00, 1, 'P', 'Female Puppy', NULL, NULL, NULL, NULL,'dog1.jpg');
INSERT INTO `items` (Item_Id, Product_Id, Item_ListPrice, Item_UnitCost, Supplier_Id, Item_Status, Item_Attr1, Item_Attr2, Item_Attr3, Item_Attr4, Item_Attr5, Item_Photo) VALUES ('EST-8', 'K9-PO-02', 18.50, 12.00, 1, 'P', 'Male Puppy', NULL, NULL, NULL, NULL,'dog2.jpg');
INSERT INTO `items` (Item_Id, Product_Id, Item_ListPrice, Item_UnitCost, Supplier_Id, Item_Status, Item_Attr1, Item_Attr2, Item_Attr3, Item_Attr4, Item_Attr5, Item_Photo) VALUES ('EST-9', 'K9-DL-01', 18.50, 12.00, 1, 'P', 'Spotless Male Puppy', NULL, NULL, NULL, NULL,'dog3.jpg');
INSERT INTO `items` (Item_Id, Product_Id, Item_ListPrice, Item_UnitCost, Supplier_Id, Item_Status, Item_Attr1, Item_Attr2, Item_Attr3, Item_Attr4, Item_Attr5, Item_Photo) VALUES ('EST-10', 'K9-DL-01', 18.50, 12.00, 1, 'P', 'Spotted Adult Female', NULL, NULL, NULL, NULL,'dog3.jpg');
INSERT INTO `items` (Item_Id, Product_Id, Item_ListPrice, Item_UnitCost, Supplier_Id, Item_Status, Item_Attr1, Item_Attr2, Item_Attr3, Item_Attr4, Item_Attr5, Item_Photo) VALUES ('EST-11', 'RP-SN-01', 18.50, 12.00, 1, 'P', 'Venomless', NULL, NULL, NULL, NULL,'reptile1.jpg');
INSERT INTO `items` (Item_Id, Product_Id, Item_ListPrice, Item_UnitCost, Supplier_Id, Item_Status, Item_Attr1, Item_Attr2, Item_Attr3, Item_Attr4, Item_Attr5, Item_Photo) VALUES ('EST-12', 'RP-SN-01', 18.50, 12.00, 1, 'P', 'Rattleless', NULL, NULL, NULL, NULL,'reptile1.jpg');
INSERT INTO `items` (Item_Id, Product_Id, Item_ListPrice, Item_UnitCost, Supplier_Id, Item_Status, Item_Attr1, Item_Attr2, Item_Attr3, Item_Attr4, Item_Attr5, Item_Photo) VALUES ('EST-13', 'RP-LI-02', 18.50, 12.00, 1, 'P', 'Green Adult', NULL, NULL, NULL, NULL,'reptile2.jpg');
INSERT INTO `items` (Item_Id, Product_Id, Item_ListPrice, Item_UnitCost, Supplier_Id, Item_Status, Item_Attr1, Item_Attr2, Item_Attr3, Item_Attr4, Item_Attr5, Item_Photo) VALUES ('EST-14', 'FL-DSH-01', 58.50, 12.00, 1, 'P', 'Tailless', NULL, NULL, NULL, NULL,'cat1.jpg');
INSERT INTO `items` (Item_Id, Product_Id, Item_ListPrice, Item_UnitCost, Supplier_Id, Item_Status, Item_Attr1, Item_Attr2, Item_Attr3, Item_Attr4, Item_Attr5, Item_Photo) VALUES ('EST-15', 'FL-DSH-01', 23.50, 12.00, 1, 'P', 'Tailed', NULL, NULL, NULL, NULL,'cat1.jpg');
INSERT INTO `items` (Item_Id, Product_Id, Item_ListPrice, Item_UnitCost, Supplier_Id, Item_Status, Item_Attr1, Item_Attr2, Item_Attr3, Item_Attr4, Item_Attr5, Item_Photo) VALUES ('EST-16', 'FL-DLH-02', 93.50, 12.00, 1, 'P', 'Adult Female', NULL, NULL, NULL, NULL,'cat2.jpg');
INSERT INTO `items` (Item_Id, Product_Id, Item_ListPrice, Item_UnitCost, Supplier_Id, Item_Status, Item_Attr1, Item_Attr2, Item_Attr3, Item_Attr4, Item_Attr5, Item_Photo) VALUES ('EST-17', 'FL-DLH-02', 93.50, 12.00, 1, 'P', 'Adult Male', NULL, NULL, NULL, NULL,'cat2.jpg');
INSERT INTO `items` (Item_Id, Product_Id, Item_ListPrice, Item_UnitCost, Supplier_Id, Item_Status, Item_Attr1, Item_Attr2, Item_Attr3, Item_Attr4, Item_Attr5, Item_Photo) VALUES ('EST-18', 'AV-CB-01', 193.50, 92.00, 1, 'P', 'Adult Male', NULL, NULL, NULL, NULL,'bird1.jpg');
INSERT INTO `items` (Item_Id, Product_Id, Item_ListPrice, Item_UnitCost, Supplier_Id, Item_Status, Item_Attr1, Item_Attr2, Item_Attr3, Item_Attr4, Item_Attr5, Item_Photo) VALUES ('EST-19', 'AV-SB-02', 15.50, 2.00, 1, 'P', 'Adult Male', NULL, NULL, NULL, NULL,'bird2.jpg');
INSERT INTO `items` (Item_Id, Product_Id, Item_ListPrice, Item_UnitCost, Supplier_Id, Item_Status, Item_Attr1, Item_Attr2, Item_Attr3, Item_Attr4, Item_Attr5, Item_Photo) VALUES ('EST-20', 'FI-FW-02', 5.50, 2.00, 1, 'P', 'Adult Male', NULL, NULL, NULL, NULL,'fish4.jpg');
INSERT INTO `items` (Item_Id, Product_Id, Item_ListPrice, Item_UnitCost, Supplier_Id, Item_Status, Item_Attr1, Item_Attr2, Item_Attr3, Item_Attr4, Item_Attr5, Item_Photo) VALUES ('EST-21', 'FI-FW-02', 5.29, 1.00, 1, 'P', 'Adult Female', NULL, NULL, NULL, NULL,'fish4.jpg');
INSERT INTO `items` (Item_Id, Product_Id, Item_ListPrice, Item_UnitCost, Supplier_Id, Item_Status, Item_Attr1, Item_Attr2, Item_Attr3, Item_Attr4, Item_Attr5, Item_Photo) VALUES ('EST-22', 'K9-RT-02', 135.50, 100.00, 1, 'P', 'Adult Male', NULL, NULL, NULL, NULL,'dog5.jpg');
INSERT INTO `items` (Item_Id, Product_Id, Item_ListPrice, Item_UnitCost, Supplier_Id, Item_Status, Item_Attr1, Item_Attr2, Item_Attr3, Item_Attr4, Item_Attr5, Item_Photo) VALUES ('EST-23', 'K9-RT-02', 145.49, 100.00, 1, 'P', 'Adult Female', NULL, NULL, NULL, NULL,'dog5.jpg');
INSERT INTO `items` (Item_Id, Product_Id, Item_ListPrice, Item_UnitCost, Supplier_Id, Item_Status, Item_Attr1, Item_Attr2, Item_Attr3, Item_Attr4, Item_Attr5, Item_Photo) VALUES ('EST-24', 'K9-RT-02', 255.50, 92.00, 1, 'P', 'Adult Male', NULL, NULL, NULL, NULL,'dog5.jpg');
INSERT INTO `items` (Item_Id, Product_Id, Item_ListPrice, Item_UnitCost, Supplier_Id, Item_Status, Item_Attr1, Item_Attr2, Item_Attr3, Item_Attr4, Item_Attr5, Item_Photo) VALUES ('EST-25', 'K9-RT-02', 325.29, 90.00, 1, 'P', 'Adult Female', NULL, NULL, NULL, NULL,'dog5.jpg');
INSERT INTO `items` (Item_Id, Product_Id, Item_ListPrice, Item_UnitCost, Supplier_Id, Item_Status, Item_Attr1, Item_Attr2, Item_Attr3, Item_Attr4, Item_Attr5, Item_Photo) VALUES ('EST-26', 'K9-CW-01', 125.50, 92.00, 1, 'P', 'Adult Male', NULL, NULL, NULL, NULL,'dog6.jpg');
INSERT INTO `items` (Item_Id, Product_Id, Item_ListPrice, Item_UnitCost, Supplier_Id, Item_Status, Item_Attr1, Item_Attr2, Item_Attr3, Item_Attr4, Item_Attr5, Item_Photo) VALUES ('EST-27', 'K9-CW-01', 155.29, 90.00, 1, 'P', 'Adult Female', NULL, NULL, NULL, NULL,'dog6.jpg');
INSERT INTO `items` (Item_Id, Product_Id, Item_ListPrice, Item_UnitCost, Supplier_Id, Item_Status, Item_Attr1, Item_Attr2, Item_Attr3, Item_Attr4, Item_Attr5, Item_Photo) VALUES ('EST-28', 'K9-RT-01', 155.29, 90.00, 1, 'P', 'Adult Female', NULL, NULL, NULL, NULL,'dog4.jpg');

#
# Dumping data for table `products`
#
INSERT INTO `products` (Product_Id, Category_Id, Product_Name, Product_Description) VALUES ('AV-CB-01', 'BIRDS', 'Amazon Parrot', 'Great companion for up to 75 years');
INSERT INTO `products` (Product_Id, Category_Id, Product_Name, Product_Description) VALUES ('AV-SB-02', 'BIRDS', 'Finch', 'Great stress reliever');
INSERT INTO `products` (Product_Id, Category_Id, Product_Name, Product_Description) VALUES ('FI-FW-01', 'FISH', 'Koi', 'Freshwater fish from Japan');
INSERT INTO `products` (Product_Id, Category_Id, Product_Name, Product_Description) VALUES ('FI-FW-02', 'FISH', 'Goldfish', 'Freshwater fish from China');
INSERT INTO `products` (Product_Id, Category_Id, Product_Name, Product_Description) VALUES ('FI-SW-01', 'FISH', 'Angelfish', 'Saltwater fish from Australia');
INSERT INTO `products` (Product_Id, Category_Id, Product_Name, Product_Description) VALUES ('FI-SW-02', 'FISH', 'Tiger Shark', 'Saltwater fish from Australia');
INSERT INTO `products` (Product_Id, Category_Id, Product_Name, Product_Description) VALUES ('FL-DLH-02', 'CATS', 'Persian', 'Friendly house cat, doubles as a princess');
INSERT INTO `products` (Product_Id, Category_Id, Product_Name, Product_Description) VALUES ('FL-DSH-01', 'CATS', 'Manx', 'Great for reducing mouse populations');
INSERT INTO `products` (Product_Id, Category_Id, Product_Name, Product_Description) VALUES ('K9-BD-01', 'DOGS', 'Bulldog', 'Friendly dog from England');
INSERT INTO `products` (Product_Id, Category_Id, Product_Name, Product_Description) VALUES ('K9-CW-01', 'DOGS', 'Chihuahua', 'Great companion dog');
INSERT INTO `products` (Product_Id, Category_Id, Product_Name, Product_Description) VALUES ('K9-DL-01', 'DOGS', 'Dalmation', 'Great dog for a fire station');
INSERT INTO `products` (Product_Id, Category_Id, Product_Name, Product_Description) VALUES ('K9-PO-02', 'DOGS', 'Poodle', 'Cute dog from France');
INSERT INTO `products` (Product_Id, Category_Id, Product_Name, Product_Description) VALUES ('K9-RT-01', 'DOGS', 'Golden Retriever', 'Great family dog');
INSERT INTO `products` (Product_Id, Category_Id, Product_Name, Product_Description) VALUES ('K9-RT-02', 'DOGS', 'Labrador Retriever', 'Great hunting dog');
INSERT INTO `products` (Product_Id, Category_Id, Product_Name, Product_Description) VALUES ('RP-LI-02', 'REPTILES', 'Iguana', 'Friendly green friend');
INSERT INTO `products` (Product_Id, Category_Id, Product_Name, Product_Description) VALUES ('RP-SN-01', 'REPTILES', 'Rattlesnake', 'Doubles as a watch dog');

#
# Dumping data for table `profiles`
#
INSERT INTO `profiles` (Account_Id, Profile_LangPref, Profile_FavCategory, Profile_MyListOpt, Profile_BannerOpt) VALUES ('w', 'Japanese', 'DOGS', 1, 1);

#
# Dumping data for table `sequences`
#
INSERT INTO `sequences` (Sequence_Name, Sequence_NextId) VALUES ('OrderNum', 0);

#
# Dumping data for table `signson`
#
INSERT INTO `signson` (Account_Id, SignOn_Password) VALUES ('ibatis', 'ibatis');

#
# Dumping data for table `suppliers`
#
INSERT INTO `suppliers` (Supplier_Id, Supplier_Name, Supplier_Status, Supplier_Addr1, Supplier_Addr2, Supplier_City, Supplier_State, Supplier_Zip, Supplier_Phone) VALUES (1, 'XYZ Pets', 'AC', '600 Avon Way', '', 'Los Angeles', 'CA', '94024', '212-947-0797');
INSERT INTO `suppliers` (Supplier_Id, Supplier_Name, Supplier_Status, Supplier_Addr1, Supplier_Addr2, Supplier_City, Supplier_State, Supplier_Zip, Supplier_Phone) VALUES (2, 'ABC Pets', 'AC', '700 Abalone Way', '', 'San Francisco', 'CA', '94024', '415-947-0797');
