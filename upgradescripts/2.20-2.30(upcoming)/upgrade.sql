﻿--upgrade scripts from nopCommerce 2.20 to nopCommerce 2.30

--new locale resources
declare @resources xml
--a resource will be delete if its value is empty
set @resources='
<Language>
  <LocaleResource Name="Admin.System.SystemInfo.ASPNETInfo.Hint">
        <Value>ASP.NET info</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.System.SystemInfo.IsFullTrust.Hint">
        <Value>Is full trust level</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.System.SystemInfo.NopVersion.Hint">
        <Value>nopCommerce version</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.System.SystemInfo.OperatingSystem.Hint">
        <Value>Operating system</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.System.SystemInfo.ServerLocalTime.Hint">
        <Value>Server local time</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.System.SystemInfo.ServerTimeZone.Hint">
        <Value>Server time zone</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.System.SystemInfo.UTCTime.Hint">
        <Value>Greenwich mean time (GMT/UTC)</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.Common.ConfigurationNotRequired">
        <Value>Configuration is not required</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.Configuration.Settings.CustomerUser.CheckUsernameAvailabilityEnabled">
        <Value>Allow customers to check the availability of usernames</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.Configuration.Settings.CustomerUser.CheckUsernameAvailabilityEnabled.Hint">
        <Value>A value indicating whether customers are allowed to check the availability of usernames (when registering or changing in ''My Account'').</Value>
    </LocaleResource>
    <LocaleResource Name="Account.CheckUsernameAvailability.Available">
        <Value>Username available</Value>
    </LocaleResource>
    <LocaleResource Name="Account.CheckUsernameAvailability.CurrentUsername">
        <Value>Current username</Value>
    </LocaleResource>
    <LocaleResource Name="Account.CheckUsernameAvailability.NotAvailable">
        <Value>Username not available</Value>
    </LocaleResource>
    <LocaleResource Name="Account.CheckUsernameAvailability.Button">
        <Value>Check Availability</Value>
    </LocaleResource>
    <LocaleResource Name="Account.Login.WrongCredentials">
        <Value>The credentials provided are incorrect</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.Orders.Fields.BillingAddress.Hint">
        <Value>Billing address info</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.Orders.Fields.ShippingAddress.Hint">
        <Value>Shipping address info</Value>
    </LocaleResource>
    <LocaleResource Name="Checkout.BillingToThisAddress">
        <Value></Value>
    </LocaleResource>
    <LocaleResource Name="Checkout.BillToThisAddress">
        <Value>Bill to this address</Value>
    </LocaleResource>    
    <LocaleResource Name="Admin.Catalog.Categories.Fields.AllowCustomersToSelectPageSize">
        <Value>Allow customers to select page size</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.Catalog.Categories.Fields.AllowCustomersToSelectPageSize.Hint">
        <Value>Whether customers are allowed to select the page size from a predefined list of options.</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.Catalog.Categories.Fields.PageSizeOptions">
        <Value>Page Size options (comma separated)</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.Catalog.Categories.Fields.PageSizeOptions.Hint">
        <Value>Comma separated list of page size options (e.g. 10, 5, 15, 20). First option is the default page size if none are selected.</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.Catalog.Manufacturers.Fields.AllowCustomersToSelectPageSize">
        <Value>Allow customers to select page size</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.Catalog.Manufacturers.Fields.AllowCustomersToSelectPageSize.Hint">
        <Value>Whether customers are allowed to select the page size from a predefined list of options.</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.Catalog.Manufacturers.Fields.PageSizeOptions">
        <Value>Page Size options (comma separated)</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.Catalog.Manufacturers.Fields.PageSizeOptions.Hint">
        <Value>Comma separated list of page size options (e.g. 10, 5, 15, 20). First option is the default page size if none are selected.</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.Configuration.Settings.Catalog.ProductsByTagAllowCustomersToSelectPageSize">
        <Value>Allow customers to select ''Products by tag'' page size</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.Configuration.Settings.Catalog.ProductsByTagAllowCustomersToSelectPageSize.Hint">
        <Value>Whether customers are allowed to select the ''Products by tag'' page size from a predefined list of options.</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.Configuration.Settings.Catalog.ProductsByTagPageSizeOptions">
        <Value>''Products by tag'' Page Size options (comma separated)</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.Configuration.Settings.Catalog.ProductsByTagPageSizeOptions.Hint">
        <Value>Comma separated list of page size options (e.g. 10, 5, 15, 20). First option is the default page size if none are selected.</Value>
    </LocaleResource>
    <LocaleResource Name="Products.Tags.PageSize">
        <Value>Display</Value>
    </LocaleResource>
    <LocaleResource Name="Products.Tags.PageSize.PerPage">
        <Value>per page</Value>
    </LocaleResource>
    <LocaleResource Name="Categories.PageSize">
        <Value>Display</Value>
    </LocaleResource>
    <LocaleResource Name="Categories.PageSize.PerPage">
        <Value>per page</Value>
    </LocaleResource>
    <LocaleResource Name="Manufacturers.PageSize">
        <Value>Display</Value>
    </LocaleResource>
    <LocaleResource Name="Manufacturers.PageSize.PerPage">
        <Value>per page</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.Configuration.Countries.States.AddNew">
        <Value>Add a new state/province</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.Configuration.Countries.States.EditStateDetails">
        <Value>Edit state/province</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.Configuration.Shipping.Methods.Added">
        <Value>The new shipping method has been added successfully.</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.Configuration.Shipping.Methods.Updated">
        <Value>The shipping method has been updated successfully.</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.Configuration.Shipping.Methods.Deleted">
        <Value>The shipping method has been deleted successfully.</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.Configuration.Shipping.Methods.BackToList">
        <Value>back to shipping method list</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.Configuration.Shipping.Methods.AddNew">
        <Value>Add a new shipping method</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.Configuration.Shipping.Methods.EditMethodDetails">
        <Value>Edit shipping method details</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.Catalog.Products.Variants.Fields.SpecialPrice">
        <Value>Special price</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.Catalog.Products.Variants.Fields.SpecialPrice.Hint">
        <Value>Set a special price for the product variant. New price will be valid between start and end dates. Leave empty to ignore field.</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.Catalog.Products.Variants.Fields.SpecialPriceStartDateTimeUtc">
        <Value>Special price start date</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.Catalog.Products.Variants.Fields.SpecialPriceStartDateTimeUtc.Hint">
        <Value>The start date of the special price in Coordinated Universal Time (UTC).</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.Catalog.Products.Variants.Fields.SpecialPriceEndDateTimeUtc">
        <Value>Special price end date</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.Catalog.Products.Variants.Fields.SpecialPriceEndDateTimeUtc.Hint">
        <Value>The end date of the special price in Coordinated Universal Time (UTC).</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.Configuration.Plugins.Fields.Configure">
        <Value>Configure</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.Configuration.Plugins.Misc.Configure">
        <Value>Configure</Value>
    </LocaleResource>
    <LocaleResource Name="Admin.Configuration.Plugins.Misc.BackToList">
        <Value>back to plugin list</Value>
    </LocaleResource>
</Language>
'

CREATE TABLE #LocaleStringResourceTmp
	(
		[ResourceName] [nvarchar](200) NOT NULL,
		[ResourceValue] [nvarchar](max) NOT NULL
	)

INSERT INTO #LocaleStringResourceTmp (ResourceName, ResourceValue)
SELECT	nref.value('@Name', 'nvarchar(200)'), nref.value('Value[1]', 'nvarchar(MAX)')
FROM	@resources.nodes('//Language/LocaleResource') AS R(nref)

--do it for each existing language
DECLARE @ExistingLanguageID int
DECLARE cur_existinglanguage CURSOR FOR
SELECT [ID]
FROM [Language]
OPEN cur_existinglanguage
FETCH NEXT FROM cur_existinglanguage INTO @ExistingLanguageID
WHILE @@FETCH_STATUS = 0
BEGIN
	DECLARE @ResourceName nvarchar(200)
	DECLARE @ResourceValue nvarchar(MAX)
	DECLARE cur_localeresource CURSOR FOR
	SELECT ResourceName, ResourceValue
	FROM #LocaleStringResourceTmp
	OPEN cur_localeresource
	FETCH NEXT FROM cur_localeresource INTO @ResourceName, @ResourceValue
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF (EXISTS (SELECT 1 FROM [LocaleStringResource] WHERE LanguageID=@ExistingLanguageID AND ResourceName=@ResourceName))
		BEGIN
			UPDATE [LocaleStringResource]
			SET [ResourceValue]=@ResourceValue
			WHERE LanguageID=@ExistingLanguageID AND ResourceName=@ResourceName
		END
		ELSE 
		BEGIN
			INSERT INTO [LocaleStringResource]
			(
				[LanguageID],
				[ResourceName],
				[ResourceValue]
			)
			VALUES
			(
				@ExistingLanguageID,
				@ResourceName,
				@ResourceValue
			)
		END
		
		IF (@ResourceValue is null or @ResourceValue = '')
		BEGIN
			DELETE [LocaleStringResource]
			WHERE LanguageID=@ExistingLanguageID AND ResourceName=@ResourceName
		END
		
		FETCH NEXT FROM cur_localeresource INTO @ResourceName, @ResourceValue
	END
	CLOSE cur_localeresource
	DEALLOCATE cur_localeresource


	--fetch next language identifier
	FETCH NEXT FROM cur_existinglanguage INTO @ExistingLanguageID
END
CLOSE cur_existinglanguage
DEALLOCATE cur_existinglanguage

DROP TABLE #LocaleStringResourceTmp
GO





--new setting
IF NOT EXISTS (SELECT 1 FROM [Setting] WHERE [name] = N'commonsettings.enablehttpcompression')
BEGIN
	INSERT [Setting] ([Name], [Value])
	VALUES (N'commonsettings.enablehttpcompression', N'true')
END
GO

--customer can't be deleted until it has associated log records
IF EXISTS (SELECT 1
           FROM   sysobjects
           WHERE  name = 'Log_Customer'
           AND parent_obj = Object_id('Log')
           AND Objectproperty(id,N'IsForeignKey') = 1)
ALTER TABLE dbo.[Log]
DROP CONSTRAINT Log_Customer
GO
ALTER TABLE [dbo].[Log]  WITH CHECK ADD  CONSTRAINT [Log_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO


IF NOT EXISTS (SELECT 1 FROM [Setting] WHERE [name] = N'catalogsettings.defaultcategorypagesizeoptions')
BEGIN
	INSERT [Setting] ([Name], [Value])
	VALUES (N'catalogsettings.defaultcategorypagesizeoptions', N'4, 2, 8, 12')
END
GO

IF NOT EXISTS (SELECT 1 FROM [Setting] WHERE [name] = N'catalogsettings.defaultmanufacturerpagesizeoptions')
BEGIN
	INSERT [Setting] ([Name], [Value])
	VALUES (N'catalogsettings.defaultmanufacturerpagesizeoptions', N'4, 2, 8, 12')
END
GO

IF NOT EXISTS (SELECT 1 FROM [Setting] WHERE [name] = N'catalogsettings.productsbytagallowcustomerstoselectpagesize')
BEGIN
	INSERT [Setting] ([Name], [Value])
	VALUES (N'catalogsettings.productsbytagallowcustomerstoselectpagesize', N'True')
END
GO

IF NOT EXISTS (SELECT 1 FROM [Setting] WHERE [name] = N'catalogsettings.productsbytagpagesizeoptions')
BEGIN
	INSERT [Setting] ([Name], [Value])
	VALUES (N'catalogsettings.productsbytagpagesizeoptions', N'4, 2, 8, 12')
END
GO


--Add fields to Category
IF NOT EXISTS (SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Category]') and NAME='AllowCustomersToSelectPageSize')
BEGIN
	ALTER TABLE [dbo].[Category]
	ADD [AllowCustomersToSelectPageSize] bit NULL
END
GO

UPDATE [dbo].[Category]
SET [AllowCustomersToSelectPageSize] = 1
WHERE [AllowCustomersToSelectPageSize] IS NULL
GO

ALTER TABLE [dbo].[Category] ALTER COLUMN [AllowCustomersToSelectPageSize] bit NOT NULL
GO

IF NOT EXISTS (SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Category]') and NAME='PageSizeOptions')
BEGIN
	ALTER TABLE [dbo].[Category]
	ADD [PageSizeOptions] nvarchar(200) NULL
END
GO

UPDATE [dbo].[Category]
SET [PageSizeOptions] = N'4, 2, 8, 12'
WHERE [PageSizeOptions] IS NULL
GO

--Add fields to Manufacturer
IF NOT EXISTS (SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Manufacturer]') and NAME='AllowCustomersToSelectPageSize')
BEGIN
	ALTER TABLE [dbo].[Manufacturer]
	ADD [AllowCustomersToSelectPageSize] bit NULL
END
GO

UPDATE [dbo].[Manufacturer]
SET [AllowCustomersToSelectPageSize] = 1
WHERE [AllowCustomersToSelectPageSize] IS NULL
GO

ALTER TABLE [dbo].[Manufacturer] ALTER COLUMN [AllowCustomersToSelectPageSize] bit NOT NULL
GO

IF NOT EXISTS (SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[Manufacturer]') and NAME='PageSizeOptions')
BEGIN
	ALTER TABLE [dbo].[Manufacturer]
	ADD [PageSizeOptions] nvarchar(200) NULL
END
GO

UPDATE [dbo].[Manufacturer]
SET [PageSizeOptions] = N'4, 2, 8, 12'
WHERE [PageSizeOptions] IS NULL
GO

--Add special price support
IF NOT EXISTS (SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[ProductVariant]') and NAME='SpecialPrice')
BEGIN
	ALTER TABLE [dbo].[ProductVariant]
	ADD [SpecialPrice] decimal(18, 4) NULL
END
GO
IF NOT EXISTS (SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[ProductVariant]') and NAME='SpecialPriceStartDateTimeUtc')
BEGIN
	ALTER TABLE [dbo].[ProductVariant]
	ADD [SpecialPriceStartDateTimeUtc] datetime NULL
END
GO
IF NOT EXISTS (SELECT 1 FROM syscolumns WHERE id=object_id('[dbo].[ProductVariant]') and NAME='SpecialPriceEndDateTimeUtc')
BEGIN
	ALTER TABLE [dbo].[ProductVariant]
	ADD [SpecialPriceEndDateTimeUtc] datetime NULL
END
GO
--Update stored procedure according to new special price properties
IF EXISTS (
		SELECT *
		FROM dbo.sysobjects
		WHERE id = OBJECT_ID(N'[dbo].[ProductLoadAllPaged]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[ProductLoadAllPaged]
GO
CREATE PROCEDURE [dbo].[ProductLoadAllPaged]
(
	@CategoryId			int = 0,
	@ManufacturerId		int = 0,
	@ProductTagId		int = 0,
	@FeaturedProducts	bit = null,	--0 featured only , 1 not featured only, null - load all products
	@PriceMin			decimal(18, 4) = null,
	@PriceMax			decimal(18, 4) = null,
	@Keywords			nvarchar(MAX) = null,
	@SearchDescriptions bit = 0,
	@FilteredSpecs		nvarchar(300) = null,	--filter by attributes (comma-separated list). e.g. 14,15,16
	@LanguageId			int = 0,
	@OrderBy			int = 0, --0 position, 5 - Name, 10 - Price, 15 - creation date
	@PageIndex			int = 0, 
	@PageSize			int = 2147483644,
	@ShowHidden			bit = 0,
	@TotalRecords		int = null OUTPUT
)
AS
BEGIN
	
	--init
	DECLARE @SearchKeywords bit
	SET @SearchKeywords = 1
	IF (@Keywords IS NULL OR @Keywords = N'')
		SET @SearchKeywords = 0

	SET @Keywords = isnull(@Keywords, '')
	SET @Keywords = '%' + rtrim(ltrim(@Keywords)) + '%'

	--filter by attributes
	SET @FilteredSpecs = isnull(@FilteredSpecs, '')
	CREATE TABLE #FilteredSpecs
	(
		SpecificationAttributeOptionId int not null
	)
	INSERT INTO #FilteredSpecs (SpecificationAttributeOptionId)
	SELECT CAST(data as int) FROM dbo.[nop_splitstring_to_table](@FilteredSpecs, ',');
	
	DECLARE @SpecAttributesCount int	
	SELECT @SpecAttributesCount = COUNT(1) FROM #FilteredSpecs

	--paging
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1
	
	CREATE TABLE #DisplayOrderTmp 
	(
		[Id] int IDENTITY (1, 1) NOT NULL,
		[ProductId] int NOT NULL
	)

	INSERT INTO #DisplayOrderTmp ([ProductId])
	SELECT p.Id
	FROM Product p with (NOLOCK) 
	LEFT OUTER JOIN Product_Category_Mapping pcm with (NOLOCK) ON p.Id=pcm.ProductId
	LEFT OUTER JOIN Product_Manufacturer_Mapping pmm with (NOLOCK) ON p.Id=pmm.ProductId
	LEFT OUTER JOIN Product_ProductTag_Mapping pptm with (NOLOCK) ON p.Id=pptm.Product_Id
	LEFT OUTER JOIN ProductVariant pv with (NOLOCK) ON p.Id = pv.ProductId
	--searching of the localized values
	--comment the line below if you don't use it. It'll improve the performance
	LEFT OUTER JOIN LocalizedProperty lp with (NOLOCK) ON p.Id = lp.EntityId AND lp.LanguageId = @LanguageId AND lp.LocaleKeyGroup = N'Product'
	WHERE 
		(
		   (
				@CategoryId IS NULL OR @CategoryId=0
				OR (pcm.CategoryId=@CategoryId AND (@FeaturedProducts IS NULL OR pcm.IsFeaturedProduct=@FeaturedProducts))
			)
		AND (
				@ManufacturerId IS NULL OR @ManufacturerId=0
				OR (pmm.ManufacturerId=@ManufacturerId AND (@FeaturedProducts IS NULL OR pmm.IsFeaturedProduct=@FeaturedProducts))
			)
		AND (
				@ProductTagId IS NULL OR @ProductTagId=0
				OR pptm.ProductTag_Id=@ProductTagId
			)
		AND	(
				@ShowHidden = 1 OR p.Published = 1
			)
		AND 
			(
				p.Deleted=0
			)
		AND 
			(
				@ShowHidden = 1 OR pv.Published = 1
			)
		AND 
			(
				@ShowHidden = 1 OR pv.Deleted = 0
			)
		AND (
				--min price
				(@PriceMin IS NULL OR @PriceMin=0)
				OR 
				(
					--special price (specified price and valid date range)
					(pv.SpecialPrice IS NOT NULL AND (getutcdate() BETWEEN isnull(pv.SpecialPriceStartDateTimeUtc, '1/1/1900') AND isnull(pv.SpecialPriceEndDateTimeUtc, '1/1/2999')))
					AND
					(pv.SpecialPrice >= @PriceMin)
				)
				OR 
				(
					--regular price (price isn't specified or date range isn't valid)
					(pv.SpecialPrice IS NULL OR (getutcdate() NOT BETWEEN isnull(pv.SpecialPriceStartDateTimeUtc, '1/1/1900') AND isnull(pv.SpecialPriceEndDateTimeUtc, '1/1/2999')))
					AND
					(pv.Price >= @PriceMin)
				)
			)
		AND (
				--max price
				(@PriceMax IS NULL OR @PriceMax=2147483644) -- max value
				OR 
				(
					--special price (specified price and valid date range)
					(pv.SpecialPrice IS NOT NULL AND (getutcdate() BETWEEN isnull(pv.SpecialPriceStartDateTimeUtc, '1/1/1900') AND isnull(pv.SpecialPriceEndDateTimeUtc, '1/1/2999')))
					AND
					(pv.SpecialPrice <= @PriceMax)
				)
				OR 
				(
					--regular price (price isn't specified or date range isn't valid)
					(pv.SpecialPrice IS NULL OR (getutcdate() NOT BETWEEN isnull(pv.SpecialPriceStartDateTimeUtc, '1/1/1900') AND isnull(pv.SpecialPriceEndDateTimeUtc, '1/1/2999')))
					AND
					(pv.Price <= @PriceMax)
				)
			)
		AND	(
				@SearchKeywords = 0 or 
				(
					-- search standard content
					patindex(@Keywords, p.name) > 0
					or patindex(@Keywords, pv.name) > 0
					or patindex(@Keywords, pv.sku) > 0
					or (@SearchDescriptions = 1 and patindex(@Keywords, p.ShortDescription) > 0)
					or (@SearchDescriptions = 1 and patindex(@Keywords, p.FullDescription) > 0)
					or (@SearchDescriptions = 1 and patindex(@Keywords, pv.Description) > 0)					
					--searching of the localized values
					--comment the lines below if you don't use it. It'll improve the performance
					or (lp.LocaleKey = N'Name' and patindex(@Keywords, lp.LocaleValue) > 0)
					or (@SearchDescriptions = 1 and lp.LocaleKey = N'ShortDescription' and patindex(@Keywords, lp.LocaleValue) > 0)
					or (@SearchDescriptions = 1 and lp.LocaleKey = N'FullDescription' and patindex(@Keywords, lp.LocaleValue) > 0)
				)
			)
		AND
			(
				@ShowHidden = 1
				OR
				(getutcdate() between isnull(pv.AvailableStartDateTimeUtc, '1/1/1900') and isnull(pv.AvailableEndDateTimeUtc, '1/1/2999'))
			)
		AND
			(
				--filter by specs
				@SpecAttributesCount = 0
				OR
				(
					NOT EXISTS(
						SELECT 1 
						FROM #FilteredSpecs [fs]
						WHERE [fs].SpecificationAttributeOptionId NOT IN (
							SELECT psam.SpecificationAttributeOptionId
							FROM dbo.Product_SpecificationAttribute_Mapping psam
							WHERE psam.AllowFiltering = 1 AND psam.ProductId = p.Id
							)
						)
					
				)
			)
		)
	ORDER BY 
		CASE WHEN @OrderBy = 0 AND @CategoryId IS NOT NULL AND @CategoryId > 0
		THEN pcm.DisplayOrder END ASC,
		CASE WHEN @OrderBy = 0 AND @ManufacturerId IS NOT NULL AND @ManufacturerId > 0
		THEN pmm.DisplayOrder END ASC,
		CASE WHEN @OrderBy = 0
		THEN p.[Name] END ASC,
		CASE WHEN @OrderBy = 5
		--THEN dbo.[nop_getnotnullnotempty](pl.[Name],p.[Name]) END ASC,
		THEN p.[Name] END ASC,
		CASE WHEN @OrderBy = 10
		THEN pv.Price END ASC,
		CASE WHEN @OrderBy = 15
		THEN p.CreatedOnUtc END DESC

	DROP TABLE #FilteredSpecs

	CREATE TABLE #PageIndex 
	(
		[IndexId] int IDENTITY (1, 1) NOT NULL,
		[ProductId] int NOT NULL
	)
	INSERT INTO #PageIndex ([ProductId])
	SELECT ProductId
	FROM #DisplayOrderTmp with (NOLOCK)
	GROUP BY ProductId
	ORDER BY min([Id])

	--total records
	SET @TotalRecords = @@rowcount
	SET ROWCOUNT @RowsToReturn
	
	DROP TABLE #DisplayOrderTmp

	--return products (returned properties should be synchronized with 'Product' entity)
	SELECT  
		p.Id,
		p.Name,
		p.ShortDescription,
		p.FullDescription,
		p.AdminComment,
		p.ProductTemplateId,
		p.ShowOnHomePage,
		p.MetaKeywords,
		p.MetaDescription,
		p.MetaTitle,
		p.SeName,
		p.AllowCustomerReviews,
		p.ApprovedRatingSum,
		p.NotApprovedRatingSum,
		p.ApprovedTotalReviews,
		p.NotApprovedTotalReviews,
		p.Published,
		p.Deleted,
		p.CreatedOnUtc,
		p.UpdatedOnUtc
	FROM
		#PageIndex [pi]
		INNER JOIN Product p with (NOLOCK) on p.Id = [pi].[ProductId]
	WHERE
		[pi].IndexId > @PageLowerBound AND 
		[pi].IndexId < @PageUpperBound
	ORDER BY
		IndexId
	
	SET ROWCOUNT 0

	DROP TABLE #PageIndex
END
GO


--scheduled tasks are stored into database now
IF NOT EXISTS (SELECT 1 FROM sysobjects WHERE id = OBJECT_ID(N'[dbo].[ScheduleTask]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[ScheduleTask](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](MAX) NOT NULL,
	[Seconds] [int] NOT NULL,
	[Type] [nvarchar](MAX) NOT NULL,
	[Enabled] [bit] NOT NULL,
	[StopOnError] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[ScheduleTask]
		WHERE [Name] = N'Send emails')
BEGIN
	INSERT [dbo].[ScheduleTask] ([Name], [Seconds], [Type], [Enabled], [StopOnError])
	VALUES (N'Send emails', 60, N'Nop.Services.Messages.QueuedMessagesSendTask, Nop.Services', 1, 0)
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[ScheduleTask]
		WHERE [Name] = N'Delete guests')
BEGIN
	INSERT [dbo].[ScheduleTask] ([Name], [Seconds], [Type], [Enabled], [StopOnError])
	VALUES (N'Delete guests', 600, N'Nop.Services.Customers.DeleteGuestsTask, Nop.Services', 1, 0)
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[ScheduleTask]
		WHERE [Name] = N'Clear cache')
BEGIN
	INSERT [dbo].[ScheduleTask] ([Name], [Seconds], [Type], [Enabled], [StopOnError])
	VALUES (N'Clear cache', 600, N'Nop.Services.Caching.ClearCacheTask, Nop.Services', 0, 0)
END
GO
IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[ScheduleTask]
		WHERE [Name] = N'Update currency exchange rates')
BEGIN
	INSERT [dbo].[ScheduleTask] ([Name], [Seconds], [Type], [Enabled], [StopOnError])
	VALUES (N'Update currency exchange rates', 900, N'Nop.Services.Directory.UpdateExchangeRateTask, Nop.Services', 1, 0)
END
GO
