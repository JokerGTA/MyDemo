using Ken_test.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ken_test.Repositories
{
    public class Ken_testContext : DbContext
    {
        public Ken_testContext(DbContextOptions<Ken_testContext> options) : base(options){}
        public virtual DbSet<UserInfo> UserInfos { get; set; }
        public virtual DbSet<MessageLog> MessageLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInfo>(e =>
            {
                e.ToTable("user_info");
                e.Property(b => b.IsDeleted).HasDefaultValue(false);
                e.HasQueryFilter(b => !b.IsDeleted);
            });

            modelBuilder.Entity<MessageLog>(e =>
            {
                e.ToTable("message_log");
                e.Property(b => b.IsDeleted).HasDefaultValue(false);
                e.HasQueryFilter(b => !b.IsDeleted);
            });
        }

        public int NotBeforeSaveChanges()
        {
            return base.SaveChanges(true);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        private void OnBeforeSaving()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                bool isHaveIsDeleted, isHaveCreateTime, isHaveEditTime, isHaveGuid;
                isHaveIsDeleted = isHaveCreateTime = isHaveEditTime = isHaveGuid = false;
                foreach (var prop in entry.CurrentValues.Properties)
                {
                    if (prop.Name.Equals("IsDeleted"))
                        isHaveIsDeleted = true;
                    if (prop.Name.Equals("CreateTime"))
                        isHaveCreateTime = true;
                    if (prop.Name.Equals("EditTime"))
                        isHaveEditTime = true;
                    if (prop.Name.Equals("Guid"))
                        isHaveGuid = true;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        if (isHaveIsDeleted) entry.CurrentValues["IsDeleted"] = false;
                        if (isHaveCreateTime) entry.CurrentValues["CreateTime"] = DateTime.Now;
                        if (isHaveGuid) entry.CurrentValues["Guid"] = Guid.NewGuid();
                        break;

                    case EntityState.Modified:
                        if (isHaveCreateTime) entry.Property("CreateTime").IsModified = false;
                        if (isHaveGuid) entry.Property("Guid").IsModified = false;
                        if (isHaveEditTime) entry.CurrentValues["EditTime"] = DateTime.Now;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        if (isHaveIsDeleted) entry.CurrentValues["IsDeleted"] = true;
                        if (isHaveEditTime) entry.CurrentValues["EditTime"] = DateTime.Now;
                        break;
                }
            }
        }

        /// <summary>
        /// 定义逻辑删除字段更新规则
        /// </summary>
        private void UnlessDeleteSave()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                bool isHaveIsDeleted, isHaveCreateTime, isHaveEditTime, isHaveGuid;
                isHaveIsDeleted = isHaveCreateTime = isHaveEditTime = isHaveGuid = false;
                foreach (var prop in entry.CurrentValues.Properties)
                {
                    if (prop.Name.Equals("IsDeleted"))
                        isHaveIsDeleted = true;
                    if (prop.Name.Equals("CreateTime"))
                        isHaveCreateTime = true;
                    if (prop.Name.Equals("EditTime"))
                        isHaveEditTime = true;
                    if (prop.Name.Equals("Guid"))
                        isHaveGuid = true;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        if (isHaveIsDeleted) entry.CurrentValues["IsDeleted"] = false;
                        if (isHaveCreateTime) entry.CurrentValues["CreateTime"] = DateTime.Now;
                        if (isHaveGuid) entry.CurrentValues["Guid"] = Guid.NewGuid();
                        break;

                    case EntityState.Modified:
                        if (isHaveCreateTime) entry.Property("CreateTime").IsModified = false;
                        if (isHaveGuid) entry.Property("Guid").IsModified = false;
                        if (isHaveEditTime) entry.CurrentValues["EditTime"] = DateTime.Now;
                        break;
                }
            }
        }
    }
    
}
