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
            new User { Id = 1, Name = "User 1", Notes = new List<Note> { new Note { Id = 1, UserId = 1, Text = "Note 1", Date = DateTime.MinValue } } },
            new User { Id = 2, Name = "User 2", Notes = new List<Note> { new Note { Id = 2, UserId = 2, Text = "Note 2", Date = DateTime.MinValue } } }
        };

        public class TestData
        { 
            public User UserData { get; set; }
            public Note NoteData { get; set; }
        }
        
        public static IEnumerable<object[]> TestCases()
        {
            yield return new object[] { new TestData { UserData = new User { Id = 3, Name = "User 3" }, NoteData = new Note { Id = 3, UserId = 1, Text = "Note 3", Date = DateTime.MinValue } } };
        }

        private void MockData()
        {
            dataProvider.Setup(dp => dp.GetAllUsers()).Returns(ExpectedUsers);

            dataProvider.Setup(dp => dp.GetUser(It.IsAny<int>())).Returns((int userId) => ExpectedUsers.Where(x => x.Id == userId).FirstOrDefault());

            dataProvider.Setup(dp => dp.GetNotesByUser(It.IsAny<int>())).Returns((int userId) => dataProvider.Object.GetAllUsers().Where(x => x.Id == userId).SelectMany(x => x.Notes));

            dataProvider.Setup(dp => dp.AddUser(It.IsAny<User>())).Returns((User user) =>
            {
                if (!ExpectedUsers.Contains(user))
                {
                    ExpectedUsers.Add(user);
                    return true;
                }
                else
                    return false;
            });

            dataProvider.Setup(dp => dp.AddNote(It.IsAny<Note>())).Returns((Note note) => 
            {
                var user = ExpectedUsers.Where(x => x.Id == note.UserId).FirstOrDefault();

                if (user != null)
                {
                    user.Notes.Add(note);
                    return true;
                }
                else
                    return false;
            });

            dataProvider.Setup(dp=> dp.RemoveUser(It.IsAny<int>())).Returns((int id)=> 
            {
                var user = ExpectedUsers.Where(x => x.Id == id).FirstOrDefault();

                if (user != null)
                {
                    ExpectedUsers.Remove(user);
                    return true;
                }
                else
                    return false;
            });

            dataProvider.Setup(dp => dp.RemoveNote(It.IsAny<int>())).Returns((int id) =>
            {
                var note = ExpectedUsers.SelectMany(x => x.Notes).Where(x => x.Id == id).FirstOrDefault();
                
                if (note != null)
                {
                    var user = ExpectedUsers.Where(x => x.Id == note.UserId).FirstOrDefault();

                    if (user != null)
                    {
                        user.Notes.Remove(note);
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            });

            dataProvider.Setup(dp => dp.UpdateUser(It.IsAny<User>())).Returns((User user) =>
            {
                var tmp = ExpectedUsers.Where(x => x.Id == user.Id).FirstOrDefault();

                if (user != null)
                {
                    tmp.Name = user.Name;                    
                    return true;
                }
                else
                    return false;
            });

            dataProvider.Setup(dp => dp.UpdateNote(It.IsAny<Note>())).Returns((Note note) =>
            {
                var tmp = ExpectedUsers.SelectMany(x => x.Notes).Where(x => x.Id == note.Id).FirstOrDefault();

                if (tmp != null)
                {
                    tmp.Text = note.Text;
                    tmp.Date = note.Date;
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
        [MemberData(nameof(TestCases))]
        public void AddUser(TestData testData)
        {
            bool result = dataProvider.Object.AddUser(testData.UserData);
            Assert.True(result);

            var expectedUser = ExpectedUsers.Where(x => x.Id == testData.UserData.Id).First();
            Assert.NotNull(expectedUser);

            Assert.Equal(expected: expectedUser, actual: testData.UserData, userComparer);
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public void AddNote(TestData testData)
        {
            bool result = dataProvider.Object.AddNote(testData.NoteData);
            Assert.True(result);

            var note = ExpectedUsers.Where(x => x.Id == testData.NoteData.UserId).SelectMany(x => x.Notes).Where(x=>x.Id == testData.NoteData.Id).FirstOrDefault();
            Assert.NotNull(note);

            Assert.Equal(expected: testData.NoteData, actual: note, noteComparer);
        }

        [Theory]
        [InlineData(1)]
        public void RemoveUser(int id)
        {
            var expectedUser1 = ExpectedUsers.Where(x => x.Id == id).First();
            Assert.NotNull(expectedUser1);

            bool result = dataProvider.Object.RemoveUser(id);
            Assert.True(result);

            var expectedUser2 = ExpectedUsers.Where(x => x.Id == id).FirstOrDefault();
            Assert.Null(expectedUser2);
        }

        [Theory]
        [InlineData(1)]
        public void RemoveNote(int id)
        {
            var expectedNote1 = ExpectedUsers.SelectMany(x=>x.Notes).Where(x => x.Id == id).FirstOrDefault();
            Assert.NotNull(expectedNote1);

            bool result = dataProvider.Object.RemoveNote(id);
            Assert.True(result);

            var expectedNote2 = ExpectedUsers.SelectMany(x => x.Notes).Where(x => x.Id == id).FirstOrDefault();
            Assert.Null(expectedNote2);
        }

        [Theory]
        [InlineData(1, "testUser")]
        public void UpdateUser(int id, string name)
        {
            var user = new User { Id = id, Name = name };
            bool result = dataProvider.Object.UpdateUser(user);
            Assert.True(result);

            var expectedUser = ExpectedUsers.Where(x => x.Id == id).FirstOrDefault();
            Assert.NotNull(expectedUser);

            Assert.Equal(expected: expectedUser, actual: user, userComparer);
        }

        [Theory]
        [InlineData(2, "testNote")]
        public void UpdateNote(int id, string text)
        {
            var note = ExpectedUsers.SelectMany(x => x.Notes).Where(x => x.Id == id).FirstOrDefault();
            Assert.NotNull(note);
                        
            var tmp = new Note
            {
                Id = note.Id,
                UserId = note.UserId,
                Date = DateTime.MaxValue,
                Text = text,
                User = note.User
            };

            bool result = dataProvider.Object.UpdateNote(tmp);
            Assert.True(result);

            var expectedNote = ExpectedUsers.SelectMany(x => x.Notes).Where(x => x.Id == id).FirstOrDefault();
            Assert.NotNull(expectedNote);

            Assert.Equal(expected: expectedNote, actual: tmp, noteComparer);
        }
    }
}