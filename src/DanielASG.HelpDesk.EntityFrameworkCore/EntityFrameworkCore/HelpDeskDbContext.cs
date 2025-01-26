using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Linq.Expressions;
using System.Text.Json;
using DanielASG.HelpDesk.Authorization.Delegation;
using DanielASG.HelpDesk.Authorization.Roles;
using DanielASG.HelpDesk.Authorization.Users;
using DanielASG.HelpDesk.Chat;
using DanielASG.HelpDesk.Contents;
using DanielASG.HelpDesk.Departments;
using DanielASG.HelpDesk.Editions;
using DanielASG.HelpDesk.ExtraProperties;
using DanielASG.HelpDesk.Friendships;
using DanielASG.HelpDesk.MultiTenancy;
using DanielASG.HelpDesk.MultiTenancy.Accounting;
using DanielASG.HelpDesk.MultiTenancy.Payments;
using DanielASG.HelpDesk.OpenIddict.Applications;
using DanielASG.HelpDesk.OpenIddict.Authorizations;
using DanielASG.HelpDesk.OpenIddict.Scopes;
using DanielASG.HelpDesk.OpenIddict.Tokens;
using DanielASG.HelpDesk.Storage;
using DanielASG.HelpDesk.Tickets;

namespace DanielASG.HelpDesk.EntityFrameworkCore
{
    public class HelpDeskDbContext : AbpZeroDbContext<Tenant, Role, User, HelpDeskDbContext>, IOpenIddictDbContext
    {
        public virtual DbSet<StandardMessage> StandardMessages { get; set; }

        public virtual DbSet<Content> Content { get; set; }

        public virtual DbSet<Ticket> Tickets { get; set; }

        public virtual DbSet<UserDepartment> UserDepartments { get; set; }

        public virtual DbSet<Department> Departments { get; set; }

        public virtual DbSet<MessageType> MessageTypes { get; set; }

        public virtual DbSet<Status> Statuses { get; set; }

        public virtual DbSet<TicketMessage> TicketMessages { get; set; }

        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<OpenIddictApplication> Applications { get; }

        public virtual DbSet<OpenIddictAuthorization> Authorizations { get; }

        public virtual DbSet<OpenIddictScope> Scopes { get; }

        public virtual DbSet<OpenIddictToken> Tokens { get; }

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<SubscriptionPaymentProduct> SubscriptionPaymentProducts { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<UserDelegation> UserDelegations { get; set; }

        public virtual DbSet<RecentPassword> RecentPasswords { get; set; }

        public HelpDeskDbContext(DbContextOptions<HelpDeskDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StandardMessage>(s =>
            {
                s.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Ticket>(t =>
                       {
                           t.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<Department>(d =>
                       {
                           d.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<MessageType>(m =>
                       {
                           m.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<Status>(s =>
                       {
                           s.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<BinaryObject>(b => { b.HasIndex(e => new { e.TenantId }); });

            modelBuilder.Entity<SubscriptionPayment>(x =>
            {
                x.Property(u => u.ExtraProperties)
                    .HasConversion(
                        d => JsonSerializer.Serialize(d, new JsonSerializerOptions()
                        {
                            WriteIndented = false
                        }),
                        s => JsonSerializer.Deserialize<ExtraPropertyDictionary>(s, new JsonSerializerOptions()
                        {
                            WriteIndented = false
                        })
                    );
            });

            modelBuilder.Entity<SubscriptionPaymentProduct>(x =>
            {
                x.Property(u => u.ExtraProperties)
                    .HasConversion(
                        d => JsonSerializer.Serialize(d, new JsonSerializerOptions()
                        {
                            WriteIndented = false
                        }),
                        s => JsonSerializer.Deserialize<ExtraPropertyDictionary>(s, new JsonSerializerOptions()
                        {
                            WriteIndented = false
                        })
                    );
            });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<UserDelegation>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.SourceUserId });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId });
            });

            modelBuilder.ConfigureOpenIddict();
        }

        protected virtual bool IsActiveFilter => CurrentUnitOfWorkProvider?.Current?.IsFilterEnabled(CustomFilterConsts.IsActive) == true;
        protected override bool ShouldFilterEntity<TEntity>(IMutableEntityType entityType)
        {
            if (typeof(IMustHaveIsActive).IsAssignableFrom(typeof(TEntity)))
            {
                return true;
            }
            if (typeof(IMustHaveCompany).IsAssignableFrom(typeof(TEntity)))
            {
                return true;
            }

            return base.ShouldFilterEntity<TEntity>(entityType);
        }

        protected override Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>()
        {
            var expression = base.CreateFilterExpression<TEntity>();
            if (typeof(IMustHaveIsActive).IsAssignableFrom(typeof(TEntity)))
            {
                Expression<Func<TEntity, bool>> mustHaveIsActive = e => ((IMustHaveIsActive)e).IsActive == true || (((IMustHaveIsActive)e).IsActive == true) == IsActiveFilter;
                expression = expression == null ? mustHaveIsActive : CombineExpressions(expression, mustHaveIsActive);
            }

            return expression;
        }
    }
}