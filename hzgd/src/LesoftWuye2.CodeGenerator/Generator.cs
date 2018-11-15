using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Extensions;
using Castle.Components.DictionaryAdapter;
using Castle.Core.Internal;
using Obs.CodeGenerator;
using Obs.Utils;

namespace LesoftWuye2.CodeGenerator
{
    public class Generator : ITransientDependency
    {
        public Log Log { get; private set; }

        public Generator(Log log)
        {
            Log = log;
        }
        public List<EntityInfoDto> GetEntities(string entityAssemblyName, string applicationProjectPath)
        {
            List<EntityInfoDto> entityInfoDtos = new EditableList<EntityInfoDto>();
            Log.Write("Start looking for an entity from:" + entityAssemblyName);
            Assembly assembly = Assembly.Load(entityAssemblyName);
            if (assembly == null)
            {
                Log.Write($"Assembly {entityAssemblyName} not exists.");
                return entityInfoDtos;
            }

            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces();
                foreach (var @interface in interfaces)
                {
                    if (false == @interface.IsGenericType) { continue; }
                    if (@interface.GetGenericTypeDefinition() == (typeof(IEntity<>)))
                    {
                        //通过特性获取表名
                        var tableAttributes = type.GetCustomAttributes(typeof(TableAttribute));
                        if (tableAttributes != null && tableAttributes.Any())
                        {
                            EntityInfoDto info = new EntityInfoDto();
                            info.Type = type;
                            info.Name = type.Name;
                            info.TableName = ((TableAttribute)(tableAttributes.First())).Name;
                            entityInfoDtos.Add(info);
                            continue;
                        }
                    }
                }
            }

            //检测实体是否有对应的代码
            foreach (var dto in entityInfoDtos)
            {
                string dir = Path.Combine(applicationProjectPath, dto.Name + "s");
                dto.IsExists = Directory.Exists(dir);
            }

            Log.Write("Total number of entities found:" + entityInfoDtos.Count);
            foreach (var dto in entityInfoDtos)
            {
                Log.Write($"[{(dto.IsExists ? "√" : " ")}] {dto.Name} {dto.TableName}");
            }

            return entityInfoDtos;
        }

        public void GenerateCode(List<EntityInfoDto> entityInfos, string applicationProjectPath, string webProjectPath, string namespaceName)
        {
            Log.Write("Ready to start generating code,Entity count:" + entityInfos.Count);
            foreach (var entityInfo in entityInfos)
            {
                Log.Write($"Entity {entityInfo.Name} is in code generation");
                GenerateApplication(entityInfo, applicationProjectPath);
                GenerateController(entityInfo, webProjectPath, namespaceName);
                GenerateWebPage(entityInfo, webProjectPath, namespaceName);
                GenerateWebPageJs(entityInfo, webProjectPath, namespaceName);
                Log.Write($"Entity {entityInfo.Name} code generate done.");
            }
        }

        #region GenerateApplication

        void GenerateApplication(EntityInfoDto entity, string applicationProjectPath)
        {
            //先生成文件夹
            string folder = Path.Combine(applicationProjectPath, entity.Name + "s");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            string dtoFolder = Path.Combine(folder, "Dto");
            if (!Directory.Exists(dtoFolder)) Directory.CreateDirectory(dtoFolder);

            GenerateApplicationDto(entity, dtoFolder);
            GenerateApplicationApplications(entity, applicationProjectPath);
        }

        void GenerateApplicationDto(EntityInfoDto entity, string dtopath)
        {

            //获取实体成员，根据特性，指定每个成员出现在哪个dto上
            //1.create
            //2.update
            //3.item
            //4.listitem

            GenerateApplicationDto_Create(entity, dtopath);
            GenerateApplicationDto_Update(entity, dtopath);
            GenerateApplicationDto_Item(entity, dtopath);
            GenerateApplicationDto_List(entity, dtopath);

        }

        void GenerateApplicationApplications(EntityInfoDto entity, string applicationProjectPath)
        {
            GenerateApplicationIApplication(entity, applicationProjectPath);
            GenerateApplicationApplication(entity, applicationProjectPath);
        }

        void GenerateApplicationIApplication(EntityInfoDto entity, string applicationProjectPath)
        {
            string namespaceName = (entity.Type.Namespace + ".Dto").Replace(".Core.", ".Application.");
            string @using = @"using System.Threading.Tasks;
                            using Abp.Application.Services;
                            using Obs.Dto;
                            using " + namespaceName + ";";

            string className = $"namespace {namespaceName.Replace(".Dto", "")} {{  public interface I{entity.Name}AppService : IApplicationService ";

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(@using);
            sb.AppendLine(className);
            sb.AppendLine("{");

            sb.AppendLine($"Task Create{entity.Name}(Create{entity.Name}Input input);");
            sb.AppendLine($"Task Delete{entity.Name}(long id);");
            sb.AppendLine($"Task Update{entity.Name}(Update{entity.Name}Input input);");
            sb.AppendLine($"Task<PageListResultDto<{entity.Name}ListDto>> Get{entity.Name}s(GetPageListRequstDto dto);");
            sb.AppendLine($"Task<{entity.Name}ItemDto> Get{entity.Name}(long id);");


            sb.AppendLine("}");
            sb.AppendLine("}");

            string content = sb.ToString();
            string fileName = Path.Combine(applicationProjectPath, entity.Name + "s", $"I{entity.Name}AppService.cs");
            File.WriteAllText(fileName, content, Encoding.UTF8);
        }

        void GenerateApplicationApplication(EntityInfoDto entity, string applicationProjectPath)
        {
            string namespaceName = (entity.Type.Namespace + ".Dto").Replace(".Core.", ".Application.");
            string @using = @"using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Obs.Dto;
using Obs.Filter;" +
                            "using " + namespaceName + ";" +
                            "using " + entity.Type.Namespace + ";";


            string className = $"namespace {namespaceName.Replace(".Dto", "")} \n {{  public class {entity.Name}AppService :{entity.Type.Namespace.Replace(".Core." + entity.Name + "s", "")}AppServiceBase,I{entity.Name}AppService ";

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(@using);
            sb.AppendLine(className);
            sb.AppendLine("{");

            string entityName = entity.Name;
            string entityName1 = entityName.ToLower();

            sb.AppendLine($"private readonly IRepository<{entityName}, long> _{entityName1}Repository;" +
                          $"public {entityName}AppService(IRepository<{entityName}, long> {entityName1}Repository){{" +
                          $"_{entityName1}Repository = {entityName1}Repository;}}");

            sb.AppendLine($"public async Task Create{entityName}(Create{entityName}Input input){{" +
                          $"var {entityName1} = input.MapTo<{entityName}>();" +
                          $"await _{entityName1}Repository.InsertAsync({entityName1});}}");

            sb.AppendLine($"public async Task Delete{entityName}(long id){{" +
                          $"await _{entityName1}Repository.DeleteAsync(id);}}");

            sb.AppendLine($"public async Task Update{entityName}(Update{entityName}Input input){{" +
                          $"var {entityName1} = await _{entityName1}Repository.GetAsync(input.Id);" +
                          $"input.MapTo({entityName1});" +
                          $"await _{entityName1}Repository.UpdateAsync({entityName1});}}");

            sb.AppendLine($"public async Task<PageListResultDto<{entityName}ListDto>> Get{entityName}s(GetPageListRequstDto dto){{" +
                          $"var query = _{entityName1}Repository.GetAll();" +
                          $"var where = FilterExpression.FindByGroup<{entityName}>(dto.Filter);" +
                          $"var count = await query.Where(where).CountAsync();" +
                          $"var list = await query.Where(where).OrderByDescending(t => t.CreationTime)" +
                          $".PageBy(dto)" +
                          $".ToListAsync();" +
                          $"var pageList = list.MapTo<List<{entityName}ListDto>>();" +
                          $"return new PageListResultDto<{entityName}ListDto>(count, pageList, dto.CurrentPage, dto.PageSize);}}");

            sb.AppendLine($"public async Task<{entityName}ItemDto> Get{entityName}(long id){{" +
                          $"var {entityName1} = await _{entityName1}Repository.GetAsync(id);" +
                          $"return {entityName1}.MapTo<{entityName}ItemDto>();}}");


            sb.AppendLine("}");
            sb.AppendLine("}");

            string content = sb.ToString();
            string fileName = Path.Combine(applicationProjectPath, entity.Name + "s", $"{entity.Name}AppService.cs");
            File.WriteAllText(fileName, content, Encoding.UTF8);
        }

        void GenerateApplicationDto_Create(EntityInfoDto entity, string dtoPath)
        {
            var members = entity.Type.GetProperties();
            StringBuilder sb = new StringBuilder();
            string lines = ""; //属性
            foreach (var member in members)
            {
                string line = "";
                var assigns = member.GetCustomAttributes(typeof(DtoAssignAttribute));
                var attributes = assigns as Attribute[] ?? assigns.ToArray();
                if (assigns == null || !attributes.Any()) continue;
                var assign = (attributes.First() as DtoAssignAttribute);
                bool hasCreate = ((assign.Targets & DtoAssignTargets.Create) != 0);
                if (!hasCreate) continue;

                line += $"public {parseToSimpleName(member.PropertyType)} {member.Name} {{get;set;}} \n\n";

                //添加特性(支持 Required,MaxLength,Display)
                if (member.HasAttribute<RequiredAttribute>())
                {
                    line = $"[Required]\n" + line;
                }

                if (member.HasAttribute<MaxLengthAttribute>())
                {
                    line = $"[MaxLength({entity.Type.Namespace}.{entity.Name}.Max{member.Name}Length)]\n" + line;
                }

                if (member.HasAttribute<DisplayAttribute>())
                {
                    var displayInfo = member.GetAttribute<DisplayAttribute>();
                    line = $"[Display(Name = \"{displayInfo.Name}\")]\n" + line;
                }
                lines += line;
            }

            string @using =
                    $" using System;\n using System.ComponentModel.DataAnnotations;\n using Abp.AutoMapper;\n using {entity.Type.Namespace};\n\n\n ";
            sb.AppendLine(@using);

            string nameSpace = (entity.Type.Namespace + ".Dto").Replace(".Core.", ".Application.");

            sb.AppendLine($"namespace {nameSpace} {{ \n [AutoMap(typeof({entity.Name}))]\n public class Create{entity.Name}Input {{\n");
            sb.AppendLine(lines);
            sb.AppendLine("}}");



            string content = sb.ToString();
            string fileName = Path.Combine(dtoPath, $"Create{entity.Name}Input.cs");
            File.WriteAllText(fileName, content, Encoding.UTF8);

        }

        void GenerateApplicationDto_Update(EntityInfoDto entity, string dtoPath)
        {
            var members = entity.Type.GetProperties();
            StringBuilder sb = new StringBuilder();
            string lines = ""; //属性
            foreach (var member in members)
            {
                string line = "";
                var assigns = member.GetCustomAttributes(typeof(DtoAssignAttribute));
                var attributes = assigns as Attribute[] ?? assigns.ToArray();
                if (assigns == null || !attributes.Any()) continue;
                var assign = (attributes.First() as DtoAssignAttribute);
                bool hasCreate = ((assign.Targets & DtoAssignTargets.Update) != 0);
                if (!hasCreate) continue;

                line += $"public {parseToSimpleName(member.PropertyType)} {member.Name} {{get;set;}} \n\n";

                //添加特性(支持 Required,MaxLength,Display)
                if (member.HasAttribute<RequiredAttribute>())
                {
                    line = $"[Required]\n" + line;
                }

                if (member.HasAttribute<MaxLengthAttribute>())
                {
                    line = $"[MaxLength({entity.Type.Namespace}.{entity.Name}.Max{member.Name}Length)]\n" + line;
                }

                if (member.HasAttribute<DisplayAttribute>())
                {
                    var displayInfo = member.GetAttribute<DisplayAttribute>();
                    line = $"[Display(Name = \"{displayInfo.Name}\")]\n" + line;
                }
                lines += line;
            }

            string @using =
                    $" using System;\n using System.ComponentModel.DataAnnotations;\n using Abp.AutoMapper;\n using {entity.Type.Namespace};\n using Abp.Application.Services.Dto;\n\n ";
            sb.AppendLine(@using);

            string nameSpace = (entity.Type.Namespace + ".Dto").Replace(".Core.", ".Application.");

            sb.AppendLine($"namespace {nameSpace} {{ \n [AutoMap(typeof({entity.Name}))]\n public class Update{entity.Name}Input:EntityDto<long>{{\n");
            sb.AppendLine(lines);
            sb.AppendLine("}}");



            string content = sb.ToString();
            string fileName = Path.Combine(dtoPath, $"Update{entity.Name}Input.cs");
            File.WriteAllText(fileName, content, Encoding.UTF8);

        }

        void GenerateApplicationDto_List(EntityInfoDto entity, string dtoPath)
        {
            var members = entity.Type.GetProperties();
            StringBuilder sb = new StringBuilder();
            string lines = ""; //属性
            foreach (var member in members)
            {
                string line = "";
                var assigns = member.GetCustomAttributes(typeof(DtoAssignAttribute));
                var attributes = assigns as Attribute[] ?? assigns.ToArray();
                if (assigns == null || !attributes.Any()) continue;
                var assign = (attributes.First() as DtoAssignAttribute);
                bool hasCreate = ((assign.Targets & DtoAssignTargets.ListItem) != 0);
                if (!hasCreate) continue;

                line += $"public {parseToSimpleName(member.PropertyType)} {member.Name} {{get;set;}} \n\n";

                //添加特性(支持 Required,MaxLength,Display)


                if (member.HasAttribute<DisplayAttribute>())
                {
                    var displayInfo = member.GetAttribute<DisplayAttribute>();
                    line = $"[Display(Name = \"{displayInfo.Name}\")]\n" + line;
                }
                lines += line;
            }

            lines += $" public DateTime CreationTime {{ get; set; }}\n\n";

            string @using =
                    $" using System;\n using System.ComponentModel.DataAnnotations;\n using Abp.AutoMapper;\n using {entity.Type.Namespace};\n using Abp.Application.Services.Dto;\n\n ";
            sb.AppendLine(@using);

            string nameSpace = (entity.Type.Namespace + ".Dto").Replace(".Core.", ".Application.");

            sb.AppendLine($"namespace {nameSpace} {{ \n [AutoMapFrom(typeof({entity.Name}))]\n public class {entity.Name}ListDto:EntityDto<long> {{\n");
            sb.AppendLine(lines);
            sb.AppendLine("}}");



            string content = sb.ToString();
            string fileName = Path.Combine(dtoPath, $"{entity.Name}ListDto.cs");
            File.WriteAllText(fileName, content, Encoding.UTF8);

        }

        void GenerateApplicationDto_Item(EntityInfoDto entity, string dtoPath)
        {
            var members = entity.Type.GetProperties();
            StringBuilder sb = new StringBuilder();
            string lines = ""; //属性
            foreach (var member in members)
            {
                string line = "";
                var assigns = member.GetCustomAttributes(typeof(DtoAssignAttribute));
                var attributes = assigns as Attribute[] ?? assigns.ToArray();
                if (assigns == null || !attributes.Any()) continue;
                var assign = (attributes.First() as DtoAssignAttribute);
                bool hasCreate = ((assign.Targets & DtoAssignTargets.Item) != 0);
                if (!hasCreate) continue;

                line += $"public {parseToSimpleName(member.PropertyType)} {member.Name} {{get;set;}} \n\n";

                //添加特性(支持 Required,MaxLength,Display)


                if (member.HasAttribute<DisplayAttribute>())
                {
                    var displayInfo = member.GetAttribute<DisplayAttribute>();
                    line = $"[Display(Name = \"{displayInfo.Name}\")]\n" + line;
                }
                lines += line;
            }

            string @using =
                    $" using System;\n using System.ComponentModel.DataAnnotations;\n using Abp.AutoMapper;\n using {entity.Type.Namespace};\n using Abp.Application.Services.Dto;\n\n ";
            sb.AppendLine(@using);

            string nameSpace = (entity.Type.Namespace + ".Dto").Replace(".Core.", ".Application.");

            sb.AppendLine($"namespace {nameSpace} {{ \n [AutoMapFrom(typeof({entity.Name}))]\n public class {entity.Name}ItemDto:EntityDto<long> {{\n");
            sb.AppendLine(lines);
            sb.AppendLine("}}");



            string content = sb.ToString();
            string fileName = Path.Combine(dtoPath, $"{entity.Name}ItemDto.cs");
            File.WriteAllText(fileName, content, Encoding.UTF8);

        }

        #endregion

        void GenerateController(EntityInfoDto entity, string webProjectPath, string namespaceName)
        {
            string namespaceNameAppp = (entity.Type.Namespace).Replace(".Core.", ".Application.");
            string @using = @"using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;" +
                            $"\nusing {namespaceName}.Application.{entity.Name}s;\n" +
                            $"using {namespaceName}.Application.{entity.Name}s.Dto;\n" +
                            "using PermissionNames = " + namespaceName + ".Core.Authorization.PermissionNames;";


            string className = $"namespace {namespaceName}.Web.Controllers \n {{  public class {entity.Name}sController :{namespaceName}ControllerBase ";

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(@using);
            sb.AppendLine(className);
            sb.AppendLine("{");

            string entityName = entity.Name;
            string entityName1 = entityName.ToLower();

            sb.AppendLine($"private readonly I{entityName}AppService _{entityName1}AppService;");

            sb.AppendLine($"public {entityName}sController(I{entityName}AppService {entityName1}AppService){{" +
                          $"_{entityName1}AppService = {entityName1}AppService;}}");

            sb.AppendLine($"public async Task<ActionResult> Index(){{" +
                          $"var output = await _{entityName1}AppService.Get{entityName}s(BuildPageListRequstDto());" +
                          $"return View(output);}}");




            sb.AppendLine("}");
            sb.AppendLine("}");

            string content = sb.ToString();
            string fileName = Path.Combine(webProjectPath, "Controllers", $"{entity.Name}Controller.cs");
            File.WriteAllText(fileName, content, Encoding.UTF8);
        }

        void GenerateWebPage(EntityInfoDto entity, string webProjectPath, string namespaceName)
        {
            string folder = Path.Combine(webProjectPath, "Views", entity.Name + "s");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            string entityDescription = entity.Name;
            if (entity.Type.HasAttribute<EntityDescriptionAttribute>())
            {
                entityDescription = entity.Type.GetAttribute<EntityDescriptionAttribute>().Description;
            }

            var pagetemplate = EmbeddedResourcesHelper.Get("LesoftWuye2.CodeGenerator", "templates.page.txt");

            //1.替换实体名
            //2.生成表格
            //3.生成新增页面
            //4.生成修改页面
            string content = pagetemplate.Replace("{$EntityDescription$}", entityDescription);
            content = content.Replace("{$NameSpace$}", namespaceName);
            content = content.Replace("{$EntityName$}", entity.Name);
            content = content.Replace("{$EntityName1$}", entity.Name.ToLower());

            string tableThead = "";
            string tableBody = "";
            var members = entity.Type.GetProperties();
            foreach (var member in members)
            {
                if (member.HasAttribute<DtoAssignAttribute>())
                {
                    var dtoassignInfo = member.GetAttribute<DtoAssignAttribute>();
                    if ((dtoassignInfo.Targets & DtoAssignTargets.ListItem) == 0) continue;
                    if (!string.IsNullOrEmpty(tableThead))
                        tableThead += Environment.NewLine;
                    if (!string.IsNullOrEmpty(tableBody))
                        tableBody += Environment.NewLine;

                    string memberTitle = member.Name;
                    if (member.HasAttribute<DisplayAttribute>())
                    {
                        var displayInfo = member.GetAttribute<DisplayAttribute>();
                        memberTitle = displayInfo.Name;
                    }

                    tableThead += $"<th class='center'>{memberTitle}</th>";
                    tableBody += $"<td class='center'>@{entity.Name.ToLower()}.{member.Name}</td>";
                }
            }



            content = content.Replace("{$TableThread$}", tableThead);
            content = content.Replace("{$TableBody$}", tableBody);



            string createModal = "";
            foreach (var member in members)
            {
                if (member.HasAttribute<DtoAssignAttribute>())
                {
                    var dtoassignInfo = member.GetAttribute<DtoAssignAttribute>();
                    if ((dtoassignInfo.Targets & DtoAssignTargets.Create) == 0) continue;
                    if (!string.IsNullOrEmpty(createModal))
                        createModal += Environment.NewLine;


                    string memberTitle = member.Name;
                    if (member.HasAttribute<DisplayAttribute>())
                    {
                        var displayInfo = member.GetAttribute<DisplayAttribute>();
                        memberTitle = displayInfo.Name;
                    }

                    bool required = member.HasAttribute<RequiredAttribute>();

                    if (member.PropertyType == typeof (string))
                    {
                        createModal +=$"<div class='form-group'><label label-for='{member.Name}'>{memberTitle}{(required? "<span class='required'>*</span>":"")}</label><input class='form-control' type='text' name='{member.Name}' {(required? "required maxlength='@"+ namespaceName + ".Core."+entity.Name+ "s." + entity.Name + ".Max" + member.Name+"Length'" : "")} ></div>";
                    }
                    else if (member.PropertyType == typeof(int) || member.PropertyType == typeof(decimal))
                    {
                        required = true;
                        createModal += $"<div class='form-group'><label label-for='{member.Name}'>{memberTitle}{(required ? "<span class='required'>*</span>" : "")}</label><input class='form-control' type='number' name='{member.Name}' {(required ? "required" : "")} ></div>";
                    }
                    else if (member.PropertyType == typeof(DateTime))
                    {
                        required = true;
                        createModal += $"<div class='form-group'><label label-for='{member.Name}'>{memberTitle}{(required ? "<span class='required'>*</span>" : "")}</label><input class='form-control date-picker' data-date-format='yyyy-mm-dd' type='text' name='{member.Name}' {(required ? "required " : "")} ></div>";
                    }
                    else if (member.PropertyType == typeof(bool))
                    {
                        createModal +=$"<div class='form-group'><div class='checkbox'><label><input type='checkbox' class='colored-blue' value='true' name='{member.Name}' data-role='excluded'><span class='text'>{memberTitle}</span></label></div></div>";
                    }
                    else
                    {
                        createModal += $"<div class='form-group'><label label-for='{member.Name}'>{memberTitle}{(required ? "<span class='required'>*</span>" : "")}</label><input class='form-control' type='text' name='{member.Name}' {(required ? "required maxlength='@" + namespaceName + ".Core." + entity.Name + "s." + entity.Name + ".Max" + member.Name + "Length'" : "")} ></div>";
                    }

                }
            }

            content = content.Replace("{$CreateModal$}", createModal);

            string updateModal = "";
            foreach (var member in members)
            {
                if (member.HasAttribute<DtoAssignAttribute>())
                {
                    var dtoassignInfo = member.GetAttribute<DtoAssignAttribute>();
                    if ((dtoassignInfo.Targets & DtoAssignTargets.Update) == 0) continue;
                    if (!string.IsNullOrEmpty(updateModal))
                        updateModal += Environment.NewLine;


                    string memberTitle = member.Name;
                    if (member.HasAttribute<DisplayAttribute>())
                    {
                        var displayInfo = member.GetAttribute<DisplayAttribute>();
                        memberTitle = displayInfo.Name;
                    }

                    bool required = member.HasAttribute<RequiredAttribute>();
                    string memberName1 = member.Name.ToCamelCase();
                    if (member.PropertyType == typeof(string))
                    {
                        updateModal += $"<div class='form-group'><label label-for='{memberName1}'>{memberTitle}{(required ? "<span class='required'>*</span>" : "")}</label><input class='form-control' type='text' name='{memberName1}' {(required ? "required maxlength='@" + namespaceName + ".Core." + entity.Name + "s." + entity.Name + ".Max" + member.Name + "Length'" : "")} ></div>";
                    }
                    else if (member.PropertyType == typeof(int) || member.PropertyType == typeof(decimal))
                    {
                        updateModal += $"<div class='form-group'><label label-for='{memberName1}'>{memberTitle}{(required ? "<span class='required'>*</span>" : "")}</label><input class='form-control' type='number' name='{memberName1}' {(required ? "required maxlength='@" + namespaceName + ".Core." + entity.Name + "s." + entity.Name + ".Max" + member.Name + "Length'" : "")} ></div>";
                    }
                    else if (member.PropertyType == typeof(DateTime))
                    {
                        required = true;
                        updateModal += $"<div class='form-group'><label label-for='{memberName1}'>{memberTitle}{(required ? "<span class='required'>*</span>" : "")}</label><input class='form-control date-picker' data-date-format='yyyy-mm-dd' type='text' name='{memberName1}' {(required ? "required " : "")} ></div>";
                    }
                    else if (member.PropertyType == typeof(bool))
                    {
                        updateModal += $"<div class='form-group'><div class='checkbox'><label><input type='checkbox' class='colored-blue' value='true' name='{memberName1}' data-role='excluded'><span class='text'>{memberTitle}</span></label></div></div>";
                    }
                    else
                    {
                        updateModal += $"<div class='form-group'><label label-for='{memberName1}'>{memberTitle}{(required ? "<span class='required'>*</span>" : "")}</label><input class='form-control' type='text' name='{memberName1}' {(required ? "required maxlength='@" + namespaceName + ".Core." + entity.Name + "s." + entity.Name + ".Max" + member.Name + "Length'" : "")} ></div>";
                    }

                }
            }

            content = content.Replace("{$UpdateModal$}", updateModal);



            string fileName = Path.Combine(folder, "Index.cshtml");
            File.WriteAllText(fileName, content, Encoding.UTF8);

        }

        void GenerateWebPageJs(EntityInfoDto entity, string webProjectPath, string namespaceName)
        {
            string folder = Path.Combine(webProjectPath, "Views", entity.Name + "s");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            string entityDescription = entity.Name;
            if (entity.Type.HasAttribute<EntityDescriptionAttribute>())
            {
                entityDescription = entity.Type.GetAttribute<EntityDescriptionAttribute>().Description;
            }

            var pagetemplate = EmbeddedResourcesHelper.Get("LesoftWuye2.CodeGenerator", "templates.js.txt");

            //1.替换实体名
            string content = pagetemplate;
            content = content.Replace("{$NameSpace$}", namespaceName);
            content = content.Replace("{$EntityName$}", entity.Name);
            content = content.Replace("{$EntityName1$}", entity.Name.ToLower());

            string fileName = Path.Combine(folder, "Index.js");
            File.WriteAllText(fileName, content, Encoding.UTF8);

        }

        string parseToSimpleName(Type type)
        {
            switch (type.FullName)
            {
                case "System.String":
                    return "string";
                case "System.Int32":
                    return "int";
                case "System.Int64":
                    return "long";
                case "System.Decimal":
                    return "decimal";
                case "System.DateTime":
                    return "DateTime";
                case "System.Boolean":
                    return "bool";
                default:
                    {
                        if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            var tttype = type.GetGenericArguments()[0];
                            switch (tttype.FullName)
                            {
                                case "System.String":
                                    return "string?";
                                case "System.Int32":
                                    return "int?";
                                case "System.Int64":
                                    return "long?";
                                case "System.Decimal":
                                    return "decimal?";
                                case "System.DateTime":
                                    return "DateTime?";
                            }
                        }
                    }
                    return "object";
            }
        }
    }
}
