using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Places.Abstract;
using Places.BLL.Mappers;
using Places.BLL.Services;
using Places.DAL.Repositories;
using Places.BLL.DTO;
using AutoFixture;
using Ninject;
using NUnit.Framework;
using NSubstitute;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

[TestClass]
public class PlaceIntegrationTests
{
    protected IKernel Kernel { get; private set; }
    private PlacesDbContext _context;
    private PlaceService _placeService;

    [TestInitialize]
    public void Setup()
    {
        // Configure in-memory database
        var options = new DbContextOptionsBuilder<PlacesDbContext>()
            .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
            .Options;
        
        _context = new PlacesDbContext(options);
        var unitOfWork = new UnitOfWork(_context);
        var mapper = new PlaceMapper();
        _placeService = new PlaceService(unitOfWork, mapper);
    }

    [TestMethod]
    public async Task AddPlace_ShouldPersistInDatabase()
    {
        // Arrange
        var placeDto = new PlaceDTO
        {
            Name = "Test Place",
            Description = "Description",
            Latitude = 50.0,
            Longitude = 30.0
        };

        // Act
        _placeService.AddPlace(placeDto);
        var placeInDb = _context.Places.FirstOrDefault(p => p.Name == "Test Place");

        // Assert
        Assert.IsNotNull(placeInDb, "Place should be persisted in the database.");
        Assert.AreEqual("Test Place", placeInDb.Name, "Place name should match.");
        Assert.AreEqual("Description", placeInDb.Description, "Place description should match.");
        Assert.AreEqual(50.0, placeInDb.Latitude, "Place latitude should match.");
        Assert.AreEqual(30.0, placeInDb.Longitude, "Place longitude should match.");
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}