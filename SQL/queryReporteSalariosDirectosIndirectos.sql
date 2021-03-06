/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 1000 [OriginId]
      ,[ContainerTrackingId]
      ,[CompanyOriginId]
  FROM [ContainersTesting].[dbo].[Origins] 
  where ContainerTrackingId = '5848'

  use [Containers]
  go

  select * from dbo.ContainersInventoryView where Numero = 'HLBU-135099-1'
  select * from dbo.ContainerTrackings c where c.ContainerTrackingId = '6032'

  select *
    into [dbo].#ContainerTrackingTmp
    from [dbo].ContainerTrackings c
   where ISNULL(c.CompanyOriginId, -1) > 0 

   declare @origin int
   declare @destination int
   declare @containerTracking int

   while((select count(*) from [dbo].#ContainerTrackingTmp) > 0)
   begin
        
		select top 1
		       @containerTracking = c.ContainerTrackingId, 
		       @origin = c.CompanyOriginId,
			   @destination = c.CompanyDestinationId 
		  from [dbo].#ContainerTrackingTmp c

		  insert into dbo.Origins(ContainerTrackingId, CompanyOriginId) values(@containerTracking, @origin)
		  insert into dbo.Destinations(ContainerTrackingId, CompanyDestinationId) values(@containerTracking, @destination)

		  delete from [dbo].#ContainerTrackingTmp where ContainerTrackingId = @containerTracking
   end
    
drop table [dbo].#ContainerTrackingTmp

