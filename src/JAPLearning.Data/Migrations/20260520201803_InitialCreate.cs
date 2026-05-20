using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JAPLearning.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "A_AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LogLevel = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(150)", nullable: false),
                    Message = table.Column<string>(type: "varchar(1000)", nullable: true),
                    StackTrace = table.Column<string>(type: "varchar(max)", nullable: true),
                    Json = table.Column<string>(type: "varchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_A_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "P_Levels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", nullable: true),
                    IsActived = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_P_Levels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "P_Modules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", nullable: true),
                    IsActived = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_P_Modules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "P_Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", nullable: true),
                    IsActived = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_P_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "P_Subjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", nullable: true),
                    IsActived = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_P_Subjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "P_Teachers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", nullable: true),
                    PhotoUrl = table.Column<string>(type: "varchar(500)", nullable: true),
                    IsActived = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_P_Teachers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "P_Teams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false),
                    Description = table.Column<string>(type: "varchar(1000)", nullable: true),
                    IsActived = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_P_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "P_Permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", nullable: true),
                    IsActived = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_P_Permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_P_Permissions_P_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "P_Modules",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "E_Articles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(150)", nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", nullable: true),
                    Content = table.Column<string>(type: "varchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "varchar(200)", nullable: true),
                    CoverImage = table.Column<string>(type: "varchar(500)", nullable: true),
                    Author = table.Column<string>(type: "varchar(100)", nullable: true),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReadingTime = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActived = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_E_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_E_Articles_P_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "P_Subjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "E_Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "varchar(100)", nullable: false),
                    LastName = table.Column<string>(type: "varchar(100)", nullable: false),
                    Email = table.Column<string>(type: "varchar(150)", nullable: false),
                    Password = table.Column<string>(type: "varchar(500)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(20)", nullable: true),
                    PhotoUrl = table.Column<string>(type: "varchar(500)", nullable: true),
                    Address = table.Column<string>(type: "varchar(255)", nullable: true),
                    Number = table.Column<string>(type: "varchar(20)", nullable: true),
                    City = table.Column<string>(type: "varchar(100)", nullable: true),
                    State = table.Column<string>(type: "varchar(100)", nullable: true),
                    Country = table.Column<string>(type: "varchar(100)", nullable: true),
                    ResetToken = table.Column<string>(type: "varchar(500)", nullable: true),
                    ResetTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActived = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_E_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_E_Users_P_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "P_Roles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_E_Users_P_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "P_Teams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "P_Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false),
                    Subtitle = table.Column<string>(type: "varchar(150)", nullable: true),
                    Description = table.Column<string>(type: "varchar(255)", nullable: true),
                    IsActived = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_P_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_P_Categories_P_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "P_Teams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "R_RolePermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_R_RolePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_R_RolePermissions_P_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "P_Permissions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_R_RolePermissions_P_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "P_Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "E_Testimonials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AuthorName = table.Column<string>(type: "varchar(100)", nullable: false),
                    Role = table.Column<string>(type: "varchar(100)", nullable: false),
                    City = table.Column<string>(type: "varchar(100)", nullable: false),
                    Country = table.Column<string>(type: "varchar(100)", nullable: false),
                    PhotoUrl = table.Column<string>(type: "varchar(500)", nullable: false),
                    LinkedinUrl = table.Column<string>(type: "varchar(500)", nullable: true),
                    Quote = table.Column<string>(type: "varchar(1000)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Featured = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActived = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_E_Testimonials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_E_Testimonials_E_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "E_Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "E_Courses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "varchar(150)", nullable: false),
                    Subtitle = table.Column<string>(type: "varchar(255)", nullable: true),
                    Description = table.Column<string>(type: "varchar(2000)", nullable: true),
                    Thumbnail = table.Column<string>(type: "varchar(500)", nullable: true),
                    PassingScore = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsBrief = table.Column<bool>(type: "bit", nullable: false),
                    IsFree = table.Column<bool>(type: "bit", nullable: false),
                    IsActived = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_E_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_E_Courses_P_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "P_Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_E_Courses_P_Levels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "P_Levels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_E_Courses_P_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "P_Teachers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "E_Certificates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CertifiedFile = table.Column<string>(type: "varchar(500)", nullable: false),
                    ValidationCode = table.Column<string>(type: "varchar(100)", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_E_Certificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_E_Certificates_E_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "E_Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_E_Certificates_E_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "E_Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "E_Topics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActived = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_E_Topics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_E_Topics_E_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "E_Courses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "R_CourseRequirements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrerequisiteCourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_R_CourseRequirements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_R_CourseRequirements_E_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "E_Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_R_CourseRequirements_E_Courses_PrerequisiteCourseId",
                        column: x => x.PrerequisiteCourseId,
                        principalTable: "E_Courses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "E_Lessons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TopicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(150)", nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", nullable: true),
                    TimeLesson = table.Column<TimeSpan>(type: "time", nullable: true),
                    Video = table.Column<string>(type: "varchar(500)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsTest = table.Column<bool>(type: "bit", nullable: false),
                    IsFreePreview = table.Column<bool>(type: "bit", nullable: false),
                    IsActived = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_E_Lessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_E_Lessons_E_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "E_Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_E_Lessons_E_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "E_Topics",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "E_Questions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LessonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(500)", nullable: false),
                    Description = table.Column<string>(type: "varchar(1000)", nullable: true),
                    IsActived = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_E_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_E_Questions_E_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "E_Lessons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "R_UserCourseLessons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LessonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WatchedSeconds = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_R_UserCourseLessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_R_UserCourseLessons_E_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "E_Lessons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_R_UserCourseLessons_E_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "E_Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "R_UserLessonTests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LessonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: true),
                    Passed = table.Column<bool>(type: "bit", nullable: false),
                    AttemptNumber = table.Column<int>(type: "int", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_R_UserLessonTests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_R_UserLessonTests_E_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "E_Lessons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_R_UserLessonTests_E_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "E_Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "E_QuestionOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(500)", nullable: false),
                    Description = table.Column<string>(type: "varchar(1000)", nullable: true),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    IsActived = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_E_QuestionOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_E_QuestionOptions_E_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "E_Questions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "R_UserLessonQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserLessonTestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SelectedOptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsRight = table.Column<bool>(type: "bit", nullable: true),
                    AnsweredDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_R_UserLessonQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_R_UserLessonQuestions_E_QuestionOptions_SelectedOptionId",
                        column: x => x.SelectedOptionId,
                        principalTable: "E_QuestionOptions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_R_UserLessonQuestions_E_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "E_Questions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_R_UserLessonQuestions_R_UserLessonTests_UserLessonTestId",
                        column: x => x.UserLessonTestId,
                        principalTable: "R_UserLessonTests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_E_Articles_Slug",
                table: "E_Articles",
                column: "Slug",
                unique: true,
                filter: "[Slug] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_E_Articles_SubjectId",
                table: "E_Articles",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_E_Certificates_CourseId",
                table: "E_Certificates",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_E_Certificates_UserId",
                table: "E_Certificates",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_E_Certificates_ValidationCode",
                table: "E_Certificates",
                column: "ValidationCode",
                unique: true,
                filter: "[ValidationCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_E_Courses_CategoryId",
                table: "E_Courses",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_E_Courses_LevelId",
                table: "E_Courses",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_E_Courses_TeacherId",
                table: "E_Courses",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_E_Lessons_CourseId",
                table: "E_Lessons",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_E_Lessons_TopicId",
                table: "E_Lessons",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_E_QuestionOptions_QuestionId",
                table: "E_QuestionOptions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_E_Questions_LessonId",
                table: "E_Questions",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_E_Testimonials_UserId",
                table: "E_Testimonials",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_E_Topics_CourseId",
                table: "E_Topics",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_E_Users_Email",
                table: "E_Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_E_Users_RoleId",
                table: "E_Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_E_Users_TeamId",
                table: "E_Users",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_P_Categories_TeamId",
                table: "P_Categories",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_P_Permissions_ModuleId",
                table: "P_Permissions",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_R_CourseRequirements_CourseId",
                table: "R_CourseRequirements",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_R_CourseRequirements_PrerequisiteCourseId",
                table: "R_CourseRequirements",
                column: "PrerequisiteCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_R_RolePermissions_PermissionId",
                table: "R_RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_R_RolePermissions_RoleId",
                table: "R_RolePermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_R_UserCourseLessons_LessonId",
                table: "R_UserCourseLessons",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_R_UserCourseLessons_UserId",
                table: "R_UserCourseLessons",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_R_UserLessonQuestions_QuestionId",
                table: "R_UserLessonQuestions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_R_UserLessonQuestions_SelectedOptionId",
                table: "R_UserLessonQuestions",
                column: "SelectedOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_R_UserLessonQuestions_UserLessonTestId",
                table: "R_UserLessonQuestions",
                column: "UserLessonTestId");

            migrationBuilder.CreateIndex(
                name: "IX_R_UserLessonTests_LessonId",
                table: "R_UserLessonTests",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_R_UserLessonTests_UserId",
                table: "R_UserLessonTests",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "A_AuditLogs");

            migrationBuilder.DropTable(
                name: "E_Articles");

            migrationBuilder.DropTable(
                name: "E_Certificates");

            migrationBuilder.DropTable(
                name: "E_Testimonials");

            migrationBuilder.DropTable(
                name: "R_CourseRequirements");

            migrationBuilder.DropTable(
                name: "R_RolePermissions");

            migrationBuilder.DropTable(
                name: "R_UserCourseLessons");

            migrationBuilder.DropTable(
                name: "R_UserLessonQuestions");

            migrationBuilder.DropTable(
                name: "P_Subjects");

            migrationBuilder.DropTable(
                name: "P_Permissions");

            migrationBuilder.DropTable(
                name: "E_QuestionOptions");

            migrationBuilder.DropTable(
                name: "R_UserLessonTests");

            migrationBuilder.DropTable(
                name: "P_Modules");

            migrationBuilder.DropTable(
                name: "E_Questions");

            migrationBuilder.DropTable(
                name: "E_Users");

            migrationBuilder.DropTable(
                name: "E_Lessons");

            migrationBuilder.DropTable(
                name: "P_Roles");

            migrationBuilder.DropTable(
                name: "E_Topics");

            migrationBuilder.DropTable(
                name: "E_Courses");

            migrationBuilder.DropTable(
                name: "P_Categories");

            migrationBuilder.DropTable(
                name: "P_Levels");

            migrationBuilder.DropTable(
                name: "P_Teachers");

            migrationBuilder.DropTable(
                name: "P_Teams");
        }
    }
}
