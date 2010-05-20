# MySQL dump 
#
# Host: localhost    Database: npetshop
# ------------------------------------------------------

#CREATE DATABASE `npetshop`;
USE `npetshop`;

#
# Table structure for table `accounts`
#
CREATE TABLE `accounts` (
  `Account_Id` varchar(20) default NULL,
  `Account_Email` varchar(80) default NULL,
  `Account_FirstName` varchar(80) default NULL,
  `Account_LastName` varchar(80) default NULL,
  `Account_Status` char(2) default NULL,
  `Account_Addr1` varchar(80) default NULL,
  `Account_Addr2` varchar(80) default NULL,
  `Account_City` varchar(80) default NULL,
  `Account_State` varchar(80) default NULL,
  `Account_Zip` varchar(20) default NULL,
  `Account_Country` varchar(20) default NULL,
  `Account_Phone` varchar(20) default NULL
) ;

#
# Table structure for table `categories`
#
CREATE TABLE `categories` (
  `Category_Id` varchar(10) default NULL,
  `Category_Name` varchar(80) default NULL,
  `Category_Description` varchar(255) default NULL
) ;

#
# Table structure for table `inventories`
#
CREATE TABLE `inventories` (
  `Item_Id` varchar(10) default NULL,
  `Inventory_Quantity` int default NULL
) ;

#
# Table structure for table `items`
#
CREATE TABLE `items` (
  `Item_Id` varchar(10) default NULL,
  `Product_Id` varchar(10) default NULL,
  `Item_ListPrice` decimal(10,2) default NULL,
  `Item_UnitCost` decimal(10,2) default NULL,
  `Supplier_Id` int default NULL,
  `Item_Status` char(2) default NULL,
  `Item_Attr1` varchar(80) default NULL,
  `Item_Attr2` varchar(80) default NULL,
  `Item_Attr3` varchar(80) default NULL,
  `Item_Attr4` varchar(80) default NULL,
  `Item_Attr5` varchar(80) default NULL,
  `Item_Photo` varchar(80) default NULL
) ;


#
# Table structure for table `linesitem`
#
CREATE TABLE `linesitem` (
  `Order_Id` int default NULL,
  `LineItem_LineNum` int default NULL,
  `Item_Id` varchar(10) default NULL,
  `LineItem_Quantity` int default NULL,
  `LineItem_UnitPrice` decimal(10,2) default NULL
) ;

#
# Table structure for table `orders`
#
CREATE TABLE `orders` (
  `Order_Id` int NOT NULL default '0',
  `Account_ID` varchar(20) default NULL,
  `Order_Date` date default NULL,
  `Order_ShipToFirstName` varchar(80) default NULL,
  `Order_ShipToLastName` varchar(80) default NULL,
  `Order_ShipAddr1` varchar(80) default NULL,
  `Order_ShipAddr2` varchar(80) default NULL,
  `Order_ShipCity` varchar(80) default NULL,
  `Order_ShipState` varchar(80) default NULL,
  `Order_ShipZip` varchar(20) default NULL,
  `Order_ShipCountry` varchar(20) default NULL,
  `Order_BillToFirstName` varchar(80) default NULL,
  `Order_BillToLastName` varchar(80) default NULL,
  `Order_BillAddr1` varchar(80) default NULL,
  `Order_BillAddr2` varchar(80) default NULL,
  `Order_BillCity` varchar(80) default NULL,
  `Order_BillState` varchar(80) default NULL,
  `Order_BillZip` varchar(20) default NULL,
  `Order_BillCountry` varchar(20) default NULL,
  `Order_TotalPrice` decimal(10,2) default NULL,
  `Order_CreditCard` varchar(20) default NULL,
  `Order_ExprDate` varchar(7) default NULL,
  `Order_CardType` varchar(40) default NULL,
  PRIMARY KEY  (`Order_Id`)
) ;

#
# Table structure for table `products`
#
CREATE TABLE `products` (
  `Product_Id` varchar(10) default NULL,
  `Category_Id` varchar(10) default NULL,
  `Product_Name` varchar(80) default NULL,
  `Product_Description` varchar(255) default NULL
) ;

#
# Table structure for table `profiles`
#
CREATE TABLE `profiles` (
  `Account_Id` varchar(20) default NULL,
  `Profile_LangPref` varchar(80) default NULL,
  `Profile_FavCategory` varchar(10) default NULL,
  `Profile_MyListOpt` bool default NULL,
  `Profile_BannerOpt` bool default NULL
) ;

#
# Table structure for table `sequences`
#
CREATE TABLE `sequences` (
  `Sequence_Name` varchar(30) default NULL,
  `Sequence_NextId` int default NULL
) ;


#
# Table structure for table `signson`
#
CREATE TABLE `signson` (
  `Account_Id` varchar(20) default NULL,
  `SignOn_Password` varchar(20) default NULL
) ;

#
# Table structure for table `suppliers`
#
CREATE TABLE `suppliers` (
  `Supplier_Id` int default NULL,
  `Supplier_Name` varchar(80) default NULL,
  `Supplier_Status` char(2) default NULL,
  `Supplier_Addr1` varchar(80) default NULL,
  `Supplier_Addr2` varchar(80) default NULL,
  `Supplier_City` varchar(80) default NULL,
  `Supplier_State` varchar(80) default NULL,
  `Supplier_Zip` varchar(5) default NULL,
  `Supplier_Phone` varchar(40) default NULL
) ;

