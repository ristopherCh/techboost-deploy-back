﻿using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TechBoost.Models;
using TechBoost.Utils;

namespace TechBoost.Repositories
{
	public class ReviewRepository : BaseRepository, IReviewRepository
	{
		public ReviewRepository(IConfiguration configuration) : base(configuration) { }

		public List<Review> GetAll()
		{
			using (var conn = Connection)
			{
				conn.Open();
				using (var cmd = conn.CreateCommand())
				{
					cmd.CommandText = @"
							SELECT Review.DateCreated, Review.Id, UserId, ResourceId, ReviewText, ReviewScore, UserProfile.Name 
							FROM Review 
							LEFT JOIN UserProfile ON Review.UserId = UserProfile.Id";

					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						var reviews = new List<Review>();
						while (reader.Read())
						{
							reviews.Add(new Review()
							{
								Id = DbUtils.GetInt(reader, "Id"),
								UserId = DbUtils.GetInt(reader, "UserId"),
								UserProfile = new UserProfile()
								{
									Name = DbUtils.GetString(reader, "Name")
								},
								ResourceId = DbUtils.GetInt(reader, "ResourceId"),
								ReviewText = DbUtils.GetString(reader, "ReviewText"),
								ReviewScore = DbUtils.GetInt(reader, "ReviewScore"),
								DateCreated = DbUtils.GetDateTime(reader, "DateCreated")
							});
						}
						return reviews;
					}
				}
			}
		}

		public List<Review> GetReviewsByResourceId(int id)
		{
			using (var conn = Connection)
			{
				conn.Open();
				using (var cmd = conn.CreateCommand())
				{
					cmd.CommandText = @"
							SELECT Review.DateCreated, Review.Id, UserId, ResourceId, ReviewText, ReviewScore, UserProfile.Name 
							FROM Review 
							LEFT JOIN UserProfile ON Review.UserId = UserProfile.Id
							WHERE ResourceId = @ResourceId";

					DbUtils.AddParameter(cmd, "@ResourceId", id);

					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						var reviews = new List<Review>();
						while (reader.Read())
						{
							reviews.Add(new Review()
							{
								Id = DbUtils.GetInt(reader, "Id"),
								UserId = DbUtils.GetInt(reader, "UserId"),
								UserProfile = new UserProfile()
								{
									Name = DbUtils.GetString(reader, "Name")
								},
								ResourceId = DbUtils.GetInt(reader, "ResourceId"),
								ReviewText = DbUtils.GetString(reader, "ReviewText"),
								ReviewScore = DbUtils.GetInt(reader, "ReviewScore"),
								DateCreated = DbUtils.GetDateTime(reader, "DateCreated")
							});
						}
						return reviews;
					}
				}
			}
		}

		public Review GetReviewById(int id)
		{
			using (var conn = Connection)
			{
				conn.Open();
				using (var cmd = conn.CreateCommand())
				{
					cmd.CommandText = @"
						SELECT Review.DateCreated, Review.Id, UserId, ResourceId, ReviewText, ReviewScore, UserProfile.Name 
							FROM Review 
							LEFT JOIN UserProfile ON Review.UserId = UserProfile.Id
                        WHERE Review.Id = @Id";

					DbUtils.AddParameter(cmd, "@Id", id);

					Review review = null;

					var reader = cmd.ExecuteReader();
					if (reader.Read())
					{
						review = new Review()
						{
							Id = DbUtils.GetInt(reader, "Id"),
							UserId = DbUtils.GetInt(reader, "UserId"),
							UserProfile = new UserProfile()
							{
								Name = DbUtils.GetString(reader, "Name")
							},
							ResourceId = DbUtils.GetInt(reader, "ResourceId"),
							ReviewText = DbUtils.GetString(reader, "ReviewText"),
							ReviewScore = DbUtils.GetInt(reader, "ReviewScore"),
							DateCreated = DbUtils.GetDateTime(reader, "DateCreated")
						};
					}
					reader.Close();

					return review;
				}
			}
		}

		public void Add(Review review)
		{
			using (var conn = Connection)
			{
				conn.Open();
				using (var cmd = conn.CreateCommand())
				{
					cmd.CommandText = @"
						INSERT INTO Review 
						(UserId, ResourceId, ReviewText, ReviewScore, DateCreated) 
									OUTPUT INSERTED.ID 
						VALUES (@UserId, @ResourceId, @ReviewText, @ReviewScore, @DateCreated)
										";
					DbUtils.AddParameter(cmd, "@UserId", review.UserId);
					DbUtils.AddParameter(cmd, "@ResourceId", review.ResourceId);
					DbUtils.AddParameter(cmd, "@ReviewText", review.ReviewText);
					DbUtils.AddParameter(cmd, "@ReviewScore", review.ReviewScore);
					DbUtils.AddParameter(cmd, "@DateCreated", review.DateCreated);

					review.Id = (int)cmd.ExecuteScalar();
				}
			}
		}
	}
}