using Abp.AutoMapper;
using DanielASG.HelpDesk.MultiTenancy.Payments.Dto;

namespace DanielASG.HelpDesk.Web.Areas.AppAreaName.Models.SubscriptionManagement;

[AutoMapFrom(typeof(SubscriptionPaymentProductDto))]
public class ShowDetailModalViewModel : SubscriptionPaymentProductDto
{
}