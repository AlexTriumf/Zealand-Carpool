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
        private const String CreateComment = "insert into Comments (UserPostID, Content, UserID,)" +
                                             " Values (@USERPOSTID, @CONTENT, @USERID)";

        private const String RemoveComment = "delete from Comments where Id = @ID ";

        private const String GetCommentString = "SELECT Comments.Id, Comments.UserPostID , Comments.Content, Comments.UserID, FROM Comments WHERE Comments.Id = @Id";
        private const String GetCommentsString = "select* from Comments where UserID = @UserID";

        public void AddComment(Comment comment)
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(CreateComment, conn))
                {

                    cmd.Parameters.AddWithValue("@USERPOSTID", comment.UserPostID);
                    cmd.Parameters.AddWithValue("@CONTENT", comment.Text);
                    cmd.Parameters.AddWithValue("@USERID", comment.UserID);



                    cmd.ExecuteNonQuery();


                }
            }
        }

        public void DeleteComment(int id)
        {
            Comment c = GetComment(id);

            using (SqlConnection conn = new SqlConnection(ConnString))
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
            using (SqlConnection conn = new SqlConnection(ConnString))
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
                        c.UserPostID = reader.GetFieldValue<User>(1);
                        c.Text = reader.GetString(2);
                        c.UserID = reader.GetFieldValue<User>(3);
   
                    }
                    return c;
                }

                throw new KeyNotFoundException("Der var ingen kommentarer med id = " + id);
            }
        }

        public List<Comment> getComments(Guid UserId)
        {
            List<Comment> list = new List<Comment>();
            using (SqlConnection conn = new SqlConnection(ConnString))
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
                        c.UserPostID = reader.GetFieldValue<User>(1);
                        c.Text = reader.GetString(2);
                        c.UserID = reader.GetFieldValue<User>(3);
                        list.Add(c);
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
