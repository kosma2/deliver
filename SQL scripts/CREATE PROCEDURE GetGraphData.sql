IF OBJECT_ID('GetGraphData', 'P') IS NOT NULL
BEGIN
    DROP PROCEDURE GetGraphData;
END
GO
CREATE PROCEDURE GetGraphData
AS
BEGIN
    DECLARE @obstPoint GEOGRAPHY = GEOGRAPHY::STPointFromText('POINT(-117.2904677012464 49.49196670168381)',4326);
    DECLARE @obst GEOGRAPHY = @obstPoint.STBuffer(300);
    CREATE TABLE #Edges
    (
        StartNodeMarkerName NVARCHAR(50),
        EndNodeMarkerName NVARCHAR(50),
        Distance FLOAT
    );
    ----
    -- Inserting non-intersecting pairs of points into the temporary table
    INSERT INTO #Edges
        (StartNodeMarkerName, EndNodeMarkerName, Distance)
    SELECT
        A.MarkerName AS StartNodeMarkerName,
        B.MarkerName AS EndNodeMarkerName,
        A.GeoLocation.STDistance(B.GeoLocation) AS Distance
    FROM
        airmarker A,
        airmarker B
    WHERE 
    A.ID != B.ID
        AND A.ShapeName = 'point'
        AND B.ShapeName = 'point'
        AND ROUND(A.GeoLocation.STDistance(B.GeoLocation), 2) < 1800
        AND GEOGRAPHY::STLineFromText('LINESTRING(' 
        + CAST(A.GeoLocation.Long AS VARCHAR(20)) + ' ' 
        + CAST(A.GeoLocation.Lat AS VARCHAR(20)) + ', ' 
        + CAST(B.GeoLocation.Long AS VARCHAR(20)) + ' ' 
        + CAST(B.GeoLocation.Lat AS VARCHAR(20)) 
        + ')', 4326).STIntersects(@obst) = 0;
    -----
    SELECT ID, MarkerName, GeoLocation.STAsText()       --return nodes
    FROM airmarker
    WHERE ShapeName = 'point';

    SELECT StartNodeMarkerName, EndNodeMarkerName, Distance --return edges
    FROM #Edges;

    DROP TABLE #Edges;
END
GO
BEGIN
    GRANT EXECUTE ON GetGraphData TO Kosma;
END
GO
