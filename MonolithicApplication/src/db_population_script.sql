CREATE TABLE [Unishop].[dbo].[app_user](
	[user_id] [uniqueidentifier] NOT NULL,
	[active] [tinyint] NULL,
	[email] [varchar](64) NOT NULL,
	[first_name] [varchar](64) NULL,
	[last_name] [varchar](64) NULL,
	[password] [varchar](255) NOT NULL,

	PRIMARY KEY ([user_id])
);


 CREATE TABLE [Unishop].[dbo].[app_unicorn](
	[unicorn_id] [uniqueidentifier] NOT NULL,
	[active] [tinyint] NULL,
	[name] [varchar](64) NULL,
	[description] [varchar](255) NULL,
	[price] [decimal](6, 2) NULL,
	[image] [varchar](255) NULL,

	PRIMARY KEY ([unicorn_id])
);


CREATE TABLE [Unishop].[dbo].[app_unicorn_basket](
	[basket_id] [uniqueidentifier] NOT NULL,
    [user_id] [uniqueidentifier] NOT NULL,
	[unicorn_id] [uniqueidentifier] NOT NULL,
	[creation_date] [datetime2](7) NOT NULL,
	[active] [tinyint] NULL,
	
	PRIMARY KEY ([basket_id])
);


INSERT [dbo].[app_unicorn] ([unicorn_id], [active], [name], [description], [price], [image]) VALUES (N'05fe0bd1-b940-4e1f-a236-4cfc98e231ce', NULL, N'UnicornFloat', N'Big Unicorn Float! Giant Glitter Unicorn Pool Floaty', CAST(100.00 AS Decimal(6, 2)), N'UnicornFloat');

INSERT [dbo].[app_unicorn] ([unicorn_id], [active], [name], [description], [price], [image]) VALUES (N'5044daba-abec-4e10-9a3d-b2272715175c', NULL, N'UnicornHipHop', N'Rainbow Hip Hop Unicorn With Sunglasses Kids Tshirt', CAST(100.00 AS Decimal(6, 2)), N'UnicornHipHop');

INSERT [dbo].[app_unicorn] ([unicorn_id], [active], [name], [description], [price], [image]) VALUES (N'2291781a-0097-4ae0-822b-36628afafca2', NULL, N'UnicornPartyDress', N'Girls Unicorn Party Dress - Tutu Pastel Rainbow Princess Power!', CAST(100.00 AS Decimal(6, 2)), N'UnicornPartyDress');

INSERT [dbo].[app_unicorn] ([unicorn_id], [active], [name], [description], [price], [image]) VALUES (N'66fd970e-c814-4722-b243-fbf3e97e0ae4', NULL, N'UnicornGlitter', N'Unicorn Glitter Backpack - Shop for Unique Unicorn Gifts for Girls!', CAST(100.00 AS Decimal(6, 2)), N'UnicornGlitter');

INSERT [dbo].[app_unicorn] ([unicorn_id], [active], [name], [description], [price], [image]) VALUES (N'ccd7c56e-1a10-402f-b085-1305aab6c497', NULL, N'UnicornBeddings', N'Rainbow Unicorn Bedding Set - The Perfect Kids or Adults Unicorn Duvet Set', CAST(100.00 AS Decimal(6, 2)), N'UnicornBeddings');

INSERT [dbo].[app_unicorn] ([unicorn_id], [active], [name], [description], [price], [image]) VALUES (N'5929ddd0-4343-40a9-9ab5-cde068e41a6d', NULL, N'UnicornPink', N'Pretty Pink Baby Unicorn Summer Party Dress', CAST(100.00 AS Decimal(6, 2)), N'UnicornPink');

INSERT [dbo].[app_unicorn] ([unicorn_id], [active], [name], [description], [price], [image]) VALUES (N'887c5538-4464-40f6-a825-e6549194fd2f', NULL, N'UnicornBackpack', N'Top Rated Classy Unicorn Backpack - Kawaii School Bag', CAST(100.00 AS Decimal(6, 2)), N'UnicornBackpack');

INSERT [dbo].[app_unicorn] ([unicorn_id], [active], [name], [description], [price], [image]) VALUES (N'a89e48f9-940d-40b2-a5e3-4837af5467cf', NULL, N'UnicornBlanket', N'Superfun Bestselling Unicorn Hooded Blanket', CAST(100.00 AS Decimal(6, 2)), N'UnicornBlanket');

INSERT [dbo].[app_unicorn] ([unicorn_id], [active], [name], [description], [price], [image]) VALUES (N'b5b16437-adeb-44f2-be15-26e25c5f4dea', NULL, N'UnicornCool', N'Cool Dabbing Unicorn Mens Hip-hop Shirts', CAST(100.00 AS Decimal(6, 2)), N'UnicornCool');

INSERT [dbo].[app_unicorn] ([unicorn_id], [active], [name], [description], [price], [image]) VALUES (N'1d6d0345-b3e5-4e0f-87a3-0a98b9a17073', NULL, N'UnicornFluffy', N'Stylish Fluffy Unicorn Slippers', CAST(100.00 AS Decimal(6, 2)), N'UnicornFluffy');

