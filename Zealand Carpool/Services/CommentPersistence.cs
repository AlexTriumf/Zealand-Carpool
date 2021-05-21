using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Zealand_Carpool.Interfaces;
using Zealand_Carpool.Models;

namespace Zealand_Carpool.Services
{
    public class CommentPersistence : IComment
    {
        string ConnString = "Data Source=andreas-zealand-server-dk.database.windows.net;Initial Catalog=Andreas-database;User ID=Andreas;Password=SecretPassword!;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private const String CreateComment = "insert into Comments (UserPostID, Content, UserID)" +
                                             " Values (@USERPOSTID, @CONTENT, @USERID)";

        private const String RemoveComment = "delete from Comments where Id = @ID";

        private const String GetCommentString = "SELECT Comments.Id, Comments.UserPostID , Comments.Content, Comments.UserID, FROM Comments WHERE Comments.Id = @Id";
        private const String GetCommentsString = "select * from Comments where UserID = @UserID ORDER BY Id DESC";
        private string _connString = "Data Source=andreas-zealand-server-dk.database.windows.net;Initial Catalog=Andreas-database;User ID=Andreas;Password=SecretPassword!;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public void AddComment(Comment comment)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(CreateComment, conn))
                {

                    cmd.Parameters.AddWithValue("@USERPOSTID", comment.UserPostID.Id);
                    cmd.Parameters.AddWithValue("@CONTENT", comment.Text);
                    cmd.Parameters.AddWithValue("@USERID", comment.UserID.Id);

                    cmd.ExecuteNonQuery();

                }
            }
        }

        public void DeleteComment(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(RemoveComment, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    int rows = cmd.ExecuteNonQuery();
                }
            }

        }

        public Comment GetComment(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(GetCommentString, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    Comment c = new Comment();

                    while (reader.Read())
                    {

                        c.Id = reader.GetInt32(0);
                        try
                        {
                            c.UserPostID = new UserPersistenceAsync().GetUser(reader.GetGuid(1)).Result;
                        }
                        catch (AggregateException)
                        {
                        }

                        c.Text = reader.GetString(2);
                        c.UserID = new UserPersistenceAsync().GetUser(reader.GetGuid(3)).Result;

                    }
                return c;
                }
                }

        }

        public List<Comment> getComments(Guid UserId)
        {
            List<Comment> list = new List<Comment>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(GetCommentsString, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", UserId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Comment c = new Comment();
                        c.Id = reader.GetInt32(0);
                        try
                        {
                            c.UserPostID = new UserPersistenceAsync().GetUser(reader.GetGuid(1)).Result;
                        }
                        catch (AggregateException)
                        {
                        }

                        if (c.UserPostID != null)
                        {
                            c.Text = reader.GetString(2);
                            c.UserID = new UserPersistenceAsync().GetUser(reader.GetGuid(3)).Result;
                            list.Add(c);
                        }

                    }
                }

                return list;
            }
        }

        public Comment MakeComment(SqlDataReader sqlReader)
        {
            Comment comment = new Comment();
            while (sqlReader.Read())
            {
                comment.Id = sqlReader.GetInt16(0);
                comment.UserPostID = sqlReader.GetFieldValue<User>(1);
                comment.Text = sqlReader.GetString(2);
                comment.UserID = sqlReader.GetFieldValue<User>(3);

            }

            return comment;
        }
    }
}
