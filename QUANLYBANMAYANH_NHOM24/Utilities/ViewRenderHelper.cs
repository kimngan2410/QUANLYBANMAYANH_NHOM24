using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace QUANLYBANMAYANH_NHOM24.Utilities
{
    public class ViewRenderHelper
    {
        private readonly ICompositeViewEngine _viewEngine;

        // Tiêm ICompositeViewEngine vào constructor
        public ViewRenderHelper(ICompositeViewEngine viewEngine)
        {
            _viewEngine = viewEngine;
        }

        public string RenderPartialViewToString(Controller controller, string viewName, object model)
        {
            controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                // Thay vì ControllerContext, bạn sử dụng _viewEngine
                var viewResult = _viewEngine.FindView(controller.ControllerContext, viewName, false);

                if (!viewResult.Success)
                {
                    return $"View {viewName} không tìm thấy.";
                }

                var viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    sw,
                    new HtmlHelperOptions()
                );

                viewResult.View.RenderAsync(viewContext).Wait();
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}
