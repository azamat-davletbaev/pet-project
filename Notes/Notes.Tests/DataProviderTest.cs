using Moq;
using Notes.Model;
using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;
using Notes.Tests.Helper;
using System.Runtime.Serialization;

namespace Notes.Tests
{
    // Тестовый класс для проверки IDataProvider
    public class DataProviderTests
    {
        private Mock<IDataProvider> dataProvider = new Mock<IDataProvider>();
        private CustomUserComparer userComparer = new CustomUserComparer();
        private CustomNoteComparer noteComparer = new CustomNoteComparer();
        public DataProviderTests()
        {
            MockData();
        }

        private static List<User> ExpectedUsers = new List<User> 
        {
            new User { Id = 1, Name = "User 1", Notes = new List<Note> { new Note { UserId = 1, Text = "Note 1" } } },
            new User { Id = 2, Name = "User 2", Notes = new List<Note> { new Note { UserId = 2, Text = "Note 2" } } }
        };

        private static List<User> ActualUsers = new List<User>
        {
            new User { Id = 1, Name = "User 1", Notes = new List<Note> { new Note { UserId = 1, Text = "Note 1" } } },
            new User { Id = 2, Name = "User 2", Notes = new List<Note> { new Note { UserId = 2, Text = "Note 2" } } }
        };
                
        public static IEnumerable<object[]> AddedUsers => new List<object[]> 
        {
            new object[] { new User { Id = 3, Name = "User 3", Notes = new List<Note> { new Note { UserId = 3, Text = "Note 3" } } } }
        };

        private void MockData()
        {
            dataProvider.Setup(dp => dp.GetAllUsers()).Returns(ActualUsers);

            dataProvider.Setup(dp => dp.GetUser(It.IsAny<int>())).Returns((int userId) => ActualUsers.Where(x => x.Id == userId).FirstOrDefault());

            dataProvider.Setup(dp => dp.GetNotesByUser(It.IsAny<int>())).Returns((int userId) => dataProvider.Object.GetAllUsers().Where(x => x.Id == userId).SelectMany(x=>x.Notes));

            dataProvider.Setup(dp => dp.AddUser(It.IsAny<User>())).Returns((User user) =>
            {
                if (!ActualUsers.Contains(user))
                {
                    ActualUsers.Add(user);
                    return true;
                }
                else
                    return false;                
            });
        }


        [Fact]
        public void GetAllUsers()
        {
            var users = dataProvider.Object.GetAllUsers();

            Assert.NotEmpty(users);
            Assert.Equal(expected: ExpectedUsers, actual: users, userComparer);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetUser(int userId)
        {
            var expectedUser = ExpectedUsers.Where(x => x.Id == userId).First();

            var user = dataProvider.Object.GetUser(userId);
            Assert.NotNull(user);
            Assert.Equal(expected: expectedUser, actual: user, userComparer);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetNotesByUser(int userId)
        {
            var expectedNotes = ExpectedUsers.Where(x => x.Id == userId).SelectMany(n => n.Notes);

            var notes = dataProvider.Object.GetNotesByUser(userId);
            Assert.NotEmpty(notes);
            Assert.Equal(expected: expectedNotes, actual: notes, noteComparer);
        }

        [Theory]
        [MemberData(nameof(AddedUsers))]
        public void AddUser(User user)
        {
            bool result = dataProvider.Object.AddUser(user);
            Assert.True(result);

            var expectedUser = ActualUsers.Where(x => x.Id == user.Id).First();
            Assert.NotNull(expectedUser);

            Assert.Equal(expected: expectedUser, actual: user, userComparer);
        }

    }
}