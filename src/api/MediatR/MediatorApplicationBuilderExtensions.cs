using DDDEastAnglia.Api.MediatR;

namespace Microsoft.AspNetCore.Builder {
    public static class MediatorApplicationBuilderExtensions {
        public static void UseMediator(this IApplicationBuilder app) {

            app.UseMiddleware<MediatorMiddleware>();

        }
    }
}
