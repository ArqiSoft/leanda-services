using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using Sds.Osdr.BddTests.FluentAssersions;
using Sds.Osdr.BddTests.Traits;
using Sds.Osdr.Generic.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sds.Osdr.BddTests
{
    [Collection("OSDR Test Harness")]
    public class PredictPropertiesValidCase : OsdrTest
    {
        private Guid BlobId { get { return GetBlobId(FolderId); } }
        private Guid FolderId { get; set; }

        public PredictPropertiesValidCase(OsdrTestHarness fixture) : base(fixture)
        {
            FolderId = PredictProperties(JohnId.ToString(), "combined lysomotrophic.sdf", new Dictionary<string, object>() { { "parentId", JohnId } }).Result;
        }

        [Fact, ProcessingTrait(TraitGroup.All, TraitGroup.MachineLearning)]
        public async Task MlPrediction_ThereAreNoErrors()
        {
            Fixture.GetFaults().Should().BeEmpty();

            await Task.CompletedTask;
        }

        [Fact, ProcessingTrait(TraitGroup.All, TraitGroup.MachineLearning)]
        public async Task MlPrediction_SingleCsvFileProcessed()
        {
            var fileId = Fixture.GetDependentFiles(FolderId).SingleOrDefault();
            fileId.Should().NotBe(Guid.Empty);

            var file = await Session.Get<File>(fileId);
            file.Should().ShouldBeEquivalentTo(new
            {
                Id = fileId,
                ParentId = FolderId,
                Status = FolderStatus.Processed,
                IsDeleted = false
            }, options => options.ExcludingMissingMembers()
            );

            await Task.CompletedTask;
        }

        [Fact, ProcessingTrait(TraitGroup.All, TraitGroup.MachineLearning)]
        public async Task MlPrediction_PredictionCsvFilePersisted()
        {
            var fileId = Fixture.GetDependentFiles(FolderId).SingleOrDefault();
            fileId.Should().NotBe(Guid.Empty);

            var file = await Session.Get<File>(fileId);

            var fileNode = Nodes.Find(new BsonDocument("_id", fileId)).FirstOrDefault() as IDictionary<string, object>;
            fileNode.Should().NotBeNull();
            fileNode.Should().NodeShouldBeEquivalentTo(file);

            var fileEntity = Files.Find(new BsonDocument("_id", fileId)).FirstOrDefault() as IDictionary<string, object>;
            fileEntity.Should().NotBeNull();
            fileEntity.Should().EntityShouldBeEquivalentTo(file);

            await Task.CompletedTask;
        }


        //[Fact, ProcessingTrait(TraitGroup.All, TraitGroup.MachineLearning)]
        //public async Task MlProcessing_PredicProperties_GeneratesAppropriateModels()
        //{
        //    var datasetFileName = "combined lysomotrophic.sdf";
        //    var blobId = await AddBlob(UserId.ToString(), datasetFileName, new Dictionary<string, object>() { { "parentId", UserId } });
        //    blobId.Should().NotBeEmpty();

        //    var modelBlobId = Guid.NewGuid();
        //    var predictionFolderId = Guid.NewGuid();
        //    var predictionId = Guid.NewGuid();
        //    var correlationId = Guid.NewGuid();
        //    var folderName = "Prediction folder";

        //    await Harness.Bus.Publish(new CreatePrediction(
        //        id: predictionId,
        //        correlationId: correlationId,
        //        parentId: UserId,
        //        folderId: predictionFolderId,
        //        folderName: folderName,
        //        datasetBlobId: blobId,
        //        datasetBucket: UserId.ToString(),
        //        modelBlobId: modelBlobId,
        //        modelBucket: UserId.ToString(),
        //        userId: UserId
        //    ));
        //    var res = await Harness.WaitWhileAllProcessed();
        //    res.Should().BeTrue();

        //    var folderView = Folders.Find(new BsonDocument("_id", predictionFolderId)).FirstOrDefault() as IDictionary<string, object>;
        //    folderView.Should().NotBeNull();

        //    var folder = await Session.Get<Folder>((Guid)predictionFolderId);
        //    folder.Should().NotBeNull();
        //    folder.ShouldBeEquivalentTo(new
        //    {
        //        Id = predictionFolderId,
        //        OwnedBy = UserId,
        //        CreatedBy = UserId,
        //        CreatedDateTime = DateTimeOffset.UtcNow,
        //        UpdatedBy = UserId,
        //        UpdatedDateTime = DateTimeOffset.UtcNow,
        //        ParentId = UserId,
        //        Name = folderName,
        //        IsDeleted = false,
        //        Status = FolderStatus.Processed
        //    }, options => options
        //        .ExcludingMissingMembers()
        //    );

        //    folderView.Should().EntityShouldBeEquivalentTo(folder);
        //}
    }
}
