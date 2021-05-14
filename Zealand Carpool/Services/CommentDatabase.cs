using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Zealand_Carpool.Interfaces;
using Zealand_Carpool.Models;

namespace Zealand_Carpool.Services
{
    public class CommentDatabase : IComment
    {
        string ConnString = "Data Source=andreas-zealand-server-dk.database.windows.net;Initial Catalog=Andreas-database;User ID=Andreas;Password=SecretPassword!;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private const String CreateComment = "insert into Comments (UserPostID, Content, UserID)" +
                                             " Values (@USERPOSTID, @CONTENT, @USERID)";

        private const String RemoveComment = "delete from Comments where Id = @ID";

        private const String GetCommentString = "SELECT Comments.Id, Comments.UserPostID , Comments.Content, Comments.UserID, FROM Comments WHERE Comments.Id = @Id";
        private const String GetCommentsString = "select * from Comments where UserID = @UserID ORDER BY Id DESC";

        public void AddComment(Comment comment)
        {
            using (SqlCommand cmd = new SqlCommand(CreateComment, DatabaseCon.Instance.SqlConnection()))
            {

                cmd.Parameters.AddWithValue("@USERPOSTID", comment.UserPostID.Id);
                cmd.Parameters.AddWithValue("@CONTENT", comment.Text);
                cmd.Parameters.AddWithValue("@USERID", comment.UserID.Id);



                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteComment(int id)
        {
            
                using (SqlCommand cmd = new SqlCommand(RemoveComment, DatabaseCon.Instance.SqlConnection()))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    int rows = cmd.ExecuteNonQuery();
                }
              
        }

        public Comment GetComment(int id)
        {
            
                using (SqlCommand cmd = new SqlCommand(GetCommentString, DatabaseCon.Instance.SqlConnection()))
                {
                    cmd.Parameters.AddWithValue("@ID", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    Comment c = new Comment();
                    
                    while (reader.Read())
                    {

                        c.Id = reader.GetInt32(0);
                        try
                        {
                            c.UserPostID = new UserDatabaseAsync().GetUser(reader.GetGuid(1)).Result;
                        }
                        catch (AggregateException)
                        {
                        }

                        c.Text = reader.GetString(2);
                        c.UserID = new UserDatabaseAsync().GetUser(reader.GetGuid(3)).Result;

                }
                    
                    return c;
                }

        }

        public List<Comment> getComments(Guid UserId)
        {
            List<Comment> list = new List<Comment>();
            using (SqlCommand cmd = new SqlCommand(GetCommentsString, DatabaseCon.Instance.SqlConnection()))
            {
                    cmd.Parameters.AddWithValue("@UserID", UserId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Comment c = new Comment();
                        c.Id = reader.GetInt32(0);
                    try
                    {
                        c.UserPostID = new UserDatabaseAsync().GetUser(reader.GetGuid(1)).Result;
                    }
                    catch (InvalidOperationException)
                    {
                    }
                        c.Text = reader.GetString(2);
                        c.UserID = new UserDatabaseAsync().GetUser(reader.GetGuid(3)).Result;
                        list.Add(c);
                    }
            }
                
            return list;
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
