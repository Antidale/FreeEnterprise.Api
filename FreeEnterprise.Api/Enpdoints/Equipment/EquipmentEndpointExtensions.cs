
namespace FreeEnterprise.Api.Enpdoints.Equipment;

public static class EquipmentEndpointExtensions
{
    public static IEndpointRouteBuilder MapEquipmentEnpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetEquipment()
            .MapGetArmor()
            .MapGetArmorByIdAsync()
            .MapGetWeaponAsync()
            .MapGetWeapons();
        return app;
    }
}
