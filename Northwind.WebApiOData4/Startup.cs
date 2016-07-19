using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Library;
using Microsoft.OData.Edm.Library.Annotations;
using Microsoft.OData.Edm.Library.Values;
using Microsoft.Spatial;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;
using System.Web.OData.Routing.Conventions;

namespace Northwind
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            System.Data.Entity.Database.SetInitializer(new Northwind.Model.initdb());
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            appBuilder.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var client = new ODataConventionModelBuilder();
            client.ContainerName = "NorthwindContext";
            client.Namespace = "JayStack";
            //client.Namespace = "Test";
            var art = client.EntitySet<Product>("Products");
            var aa = client.EntityType<Product>();

            var c = client.EntitySet<Category>("Categories");
            var cc = client.EntityType<Category>();

            var tig1 = client.EntityType<TestItemGuid>().Collection.Action("GetTitles");
            tig1.Parameter<int>("count");
            tig1.ReturnsCollection<string>();

            var tig2 = client.EntityType<TestItemGuid>().Action("GetDisplayText");

            tig2.Returns<string>();

            var tig3 = client.EntityType<TestItemGuid>().Action("Concatenate");
            tig3.CollectionParameter<string>("values");
            tig3.Returns<string>();

            var a5 = client.Action("Delete");
            var init = client.Action("InitDb");

            var a6 = client.Action("SAction1");
            a6.Parameter<int>("number");
            a6.Returns<string>();

            var a6_b = client.Action("SAction2");
            a6_b.Parameter<int>("count");
            a6_b.ReturnsCollectionFromEntitySet<Product>("Products");

            var a7 = client.Function("SFunction1");
            a7.Parameter<int>("number");
            a7.ReturnsCollection<string>();

            var a8 = client.Function("SFunction2");
            a8.Parameter<int>("number");
            a8.IncludeInServiceDocument = false;
            a8.Returns<string>();

            var a9 = client.EntityType<Category>().Collection.Function("SFunction1");
            a9.Parameter<int>("p1");
            a9.Parameter<string>("p2");
            a9.CollectionParameter<string>("p3");
            a9.ReturnsCollection<string>();

            var a10 = client.EntityType<Category>().Collection.Action("SAction1");
            a10.Parameter<int>("p1");
            a10.Parameter<string>("p2");
            a10.CollectionParameter<string>("p3");
            a10.ReturnsCollection<string>();

            client.EntityType<Product>().Ignore(a => a.Category);
            client.EntityType<Category>().Ignore(a => a.Products);


            var model = client.GetEdmModel();


            SetComputed(model, model.EntityContainer.FindEntitySet("Products").EntityType().FindProperty("Id"));
            SetComputed(model, model.EntityContainer.FindEntitySet("Categories").EntityType().FindProperty("Id"));

            var m = model as EdmModel;
            //Categories
            m.SetVocabularyAnnotation(
                new EdmAnnotation(model.EntityContainer.FindEntitySet("Categories").EntityType(),
                    new EdmTerm("UI", "DisplayName", EdmPrimitiveTypeKind.String),
                    new EdmStringConstant("Categories")));

            m.SetVocabularyAnnotation(
                new EdmAnnotation(model.EntityContainer.FindEntitySet("Categories").EntityType().FindProperty("Id"),
                    new EdmTerm("UI", "DisplayName", EdmPrimitiveTypeKind.String),
                    new EdmStringConstant("Category identifier")));
            m.SetVocabularyAnnotation(
                new EdmAnnotation(model.EntityContainer.FindEntitySet("Categories").EntityType().FindProperty("Id"),
                    new EdmTerm("UI", "ControlHint", EdmPrimitiveTypeKind.String),
                    new EdmStringConstant("ReadOnly")));

            m.SetVocabularyAnnotation(
                new EdmAnnotation(model.EntityContainer.FindEntitySet("Categories").EntityType().FindProperty("Name"),
                    new EdmTerm("UI", "DisplayName", EdmPrimitiveTypeKind.String),
                    new EdmStringConstant("Category name")));
            m.SetVocabularyAnnotation(
                new EdmAnnotation(model.EntityContainer.FindEntitySet("Categories").EntityType().FindProperty("Name"),
                    new EdmTerm("UI", "ControlHint", EdmPrimitiveTypeKind.String),
                    new EdmStringConstant("ShortText")));

            //Products
            m.SetVocabularyAnnotation(
                new EdmAnnotation(model.EntityContainer.FindEntitySet("Products").EntityType(),
                    new EdmTerm("UI", "DisplayName", EdmPrimitiveTypeKind.String),
                    new EdmStringConstant("Products")));

            m.SetVocabularyAnnotation(
                new EdmAnnotation(model.EntityContainer.FindEntitySet("Products").EntityType().FindProperty("Id"),
                    new EdmTerm("UI", "DisplayName", EdmPrimitiveTypeKind.String),
                    new EdmStringConstant("Product identifier")));
            m.SetVocabularyAnnotation(
                new EdmAnnotation(model.EntityContainer.FindEntitySet("Products").EntityType().FindProperty("Id"),
                    new EdmTerm("UI", "ControlHint", EdmPrimitiveTypeKind.String),
                    new EdmStringConstant("ReadOnly")));

            m.SetVocabularyAnnotation(
                new EdmAnnotation(model.EntityContainer.FindEntitySet("Products").EntityType().FindProperty("Name"),
                    new EdmTerm("UI", "DisplayName", EdmPrimitiveTypeKind.String),
                    new EdmStringConstant("Product title")));
            m.SetVocabularyAnnotation(
                new EdmAnnotation(model.EntityContainer.FindEntitySet("Products").EntityType().FindProperty("Name"),
                    new EdmTerm("UI", "ControlHint", EdmPrimitiveTypeKind.String),
                    new EdmStringConstant("ShortText")));

            m.SetVocabularyAnnotation(
                new EdmAnnotation(model.EntityContainer.FindEntitySet("Products").EntityType().FindProperty("QuantityPerUnit"),
                    new EdmTerm("UI", "DisplayName", EdmPrimitiveTypeKind.String),
                    new EdmStringConstant("Product English name")));
            m.SetVocabularyAnnotation(
                new EdmAnnotation(model.EntityContainer.FindEntitySet("Products").EntityType().FindProperty("QuantityPerUnit"),
                    new EdmTerm("UI", "ControlHint", EdmPrimitiveTypeKind.String),
                    new EdmStringConstant("ShortText")));

            m.SetVocabularyAnnotation(
                new EdmAnnotation(model.EntityContainer.FindEntitySet("Products").EntityType().FindProperty("UnitPrice"),
                    new EdmTerm("UI", "DisplayName", EdmPrimitiveTypeKind.Decimal),
                    new EdmStringConstant("Unit price of product")));
            m.SetVocabularyAnnotation(
                new EdmAnnotation(model.EntityContainer.FindEntitySet("Products").EntityType().FindProperty("UnitPrice"),
                    new EdmTerm("UI", "ControlHint", EdmPrimitiveTypeKind.Decimal),
                    new EdmStringConstant("Decimal")));



            UpgradeBidirectionalNavigationProperties(model, "Products", "Categories", "Northwind.Product", "Northwind.Category", "Category", "Products", EdmMultiplicity.ZeroOrOne, EdmMultiplicity.Many);


            IList<IODataRoutingConvention> conventions = ODataRoutingConventions.CreateDefaultWithAttributeRouting(config, model);
            conventions.Insert(0, new ContainmentRoutingConvention());
            
            config.MapODataServiceRoute("odata", "odata", model, new DefaultODataPathHandler(), conventions, new System.Web.OData.Batch.DefaultODataBatchHandler(new HttpServer(config)));

            appBuilder.UseWebApi(config);
        }

        public static void UpgradeBidirectionalNavigationProperties(Microsoft.OData.Edm.IEdmModel model, string setName, string partnerSetName, string elementTypeName, string partnerElementTypeName, string navigationName, string partnerName, EdmMultiplicity multipl, EdmMultiplicity partnerMultipl)
        {
            var fromSet = (EdmEntitySet)model.EntityContainer.FindEntitySet(setName);
            var partnerSet = (EdmEntitySet)model.EntityContainer.FindEntitySet(partnerSetName);

            var fromType = (EdmEntityType)model.FindDeclaredType(elementTypeName);
            var partnerType = (EdmEntityType)model.FindDeclaredType(partnerElementTypeName);

            if (fromType == null)
                throw new Exception("fromType is null");

            if (partnerType == null)
                throw new Exception("partnerType is null");

            var partsProperty = new EdmNavigationPropertyInfo();
            partsProperty.Name = navigationName;
            partsProperty.TargetMultiplicity = multipl;
            partsProperty.Target = partnerType;
            partsProperty.ContainsTarget = false;
            partsProperty.OnDelete = EdmOnDeleteAction.None;

            var partnerProperty = new EdmNavigationPropertyInfo();
            partnerProperty.Name = partnerName;
            partnerProperty.TargetMultiplicity = partnerMultipl;
            partnerProperty.Target = fromType;
            partnerProperty.ContainsTarget = false;
            partnerProperty.OnDelete = EdmOnDeleteAction.None;

            fromSet.AddNavigationTarget(fromType.AddBidirectionalNavigation(partsProperty, partnerProperty), partnerSet);
        }

        public static void SetAnnotation(IEdmModel model, IEdmProperty field, string ns, string name, EdmPrimitiveTypeKind typeKind)
        {
            var m = model as EdmModel;
            m.AddVocabularyAnnotation(
                new EdmAnnotation(field,
                    new EdmTerm(ns, name, typeKind),
                    new EdmBooleanConstant(true)));
        }

        public static void SetComputed(IEdmModel model, IEdmProperty field)
        {
            SetAnnotation(model, field, "Org.OData.Core.V1", "Computed", EdmPrimitiveTypeKind.Boolean);
        }
    }
}
