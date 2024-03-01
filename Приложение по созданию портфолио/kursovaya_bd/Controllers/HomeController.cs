using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using kursovaya_bd.Models;
using System.Text;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using System.Net.Mail;

namespace kursovaya_bd.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    IWebHostEnvironment _appEnvironment;
    public static string? userMail;
    public static int? IdProjects;

    public HomeController(ILogger<HomeController> logger, IWebHostEnvironment appEnvironment)
    {
        _logger = logger;
        _appEnvironment = appEnvironment;
    }

    //регистрация
    public IActionResult Login()
    {
        return View();
    }

    //авторизация
    public IActionResult Auth()
    {
        return View();
    }

    //главная 
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Admin(string email)
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;port=8889;username=root;password=root;database=portfolio");
        connection.Open();
        string query = $"DELETE FROM users WHERE email='{email}';";
        MySqlCommand command = new MySqlCommand(query, connection);
        int row = command.ExecuteNonQuery();
        if (row > 0)
        {
            string mess = "Аккаунт успешно удален";
            ViewBag.Message = mess;
        }
        else
        {
            string mess = "Что-то пошло не так";
            ViewBag.Message = mess;
        }
        return View();
    }

    //проекты
    public IActionResult Projects()
    {
        List<MyViewModel> projects = new List<MyViewModel>();
        MySqlConnection connection = new MySqlConnection("server=localhost;port=8889;username=root;password=root;database=portfolio");
        connection.Open();
        string query = $"select projects.id_project, projects.name_project, projects.image_path from projects;";
        MySqlCommand command = new MySqlCommand(query, connection);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            string id_project = reader.GetString(0);
            string name_project = reader.GetString(1);
            string image_path = reader.GetString(2);
            MyViewModel MyModel = new MyViewModel()
            {
                Id = Convert.ToInt32(id_project),
                Name = name_project,
                Image = image_path

            };
            projects.Add(MyModel);
        }
        return View(projects);
    }

    //страницы с проектами
    public IActionResult Project()
    {
        return View();
    }

    //личный кабинет
    public IActionResult Kabinet()
    {
        return View();
    }

    //проекты после авторизации
    public IActionResult Projects_login()
    {
        List<MyViewModel> projects = new List<MyViewModel>();
        MySqlConnection connection = new MySqlConnection("server=localhost;port=8889;username=root;password=root;database=portfolio");
        connection.Open();
        string query = $"select projects.id_project, projects.name_project, projects.image_path from projects;";
        MySqlCommand command = new MySqlCommand(query, connection);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            string id_project = reader.GetString(0);
            string name_project = reader.GetString(1);
            string image_path = reader.GetString(2);
            MyViewModel MyModel = new MyViewModel()
            {
                Id = Convert.ToInt32(id_project),
                Name = name_project,
                Image = image_path
            };
            projects.Add(MyModel);
        }
        return View(projects);
    }

    //страницы с проектами после авторизации
    public IActionResult Project_login()
    {
        return View();
    }

    //главная после авторизации
    public IActionResult Index_login()
    {
        return View();
    }

    public IActionResult Admin()
    {
        return View();
    }

    public IActionResult Show_projects()
    {
        return View();
    }

    //добавление проекта
    public IActionResult Add_project(string name_project, string skills, IFormFile image, string github)
    {
        Kabinet_add(name_project, skills, image, github);
        return View();
    }

    [HttpPost]
    public IActionResult Search_auth(string searchOption, string searchQuery)
    {
        if (searchOption == "option1")
        {
            List<MyViewModel> projects = new List<MyViewModel>();
            MySqlConnection connection = new MySqlConnection("server=localhost;port=8889;username=root;password=root;database=portfolio");
            connection.Open();
            string query = $"SELECT projects.id_project, projects.name_project, projects.image_path FROM projects WHERE projects.name_project LIKE '%{searchQuery}%';";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string id_project = reader.GetString(0);
                string name_project = reader.GetString(1);
                string image_path = reader.GetString(2);
                MyViewModel MyModel = new MyViewModel()
                {
                    Id = Convert.ToInt32(id_project),
                    Name = name_project,
                    Image = image_path
                };
                projects.Add(MyModel);
            }
            return View("Projects_login", projects);
        }
        else if (searchOption == "option2")
        {
            List<MyViewModel> projects = new List<MyViewModel>();
            MySqlConnection connection = new MySqlConnection("server=localhost;port=8889;username=root;password=root;database=portfolio");
            connection.Open();
            string query = $"SELECT projects.id_project, projects.name_project, projects.image_path FROM projects WHERE projects.skills LIKE '%{searchQuery}%';";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string id_project = reader.GetString(0);
                string name_project = reader.GetString(1);
                string image_path = reader.GetString(2);
                MyViewModel MyModel = new MyViewModel()
                {
                    Id = Convert.ToInt32(id_project),
                    Name = name_project,
                    Image = image_path
                };
                projects.Add(MyModel);
            }
            return View("Projects_login", projects);
        }
        else
        {
            List<MyViewModel> projects = new List<MyViewModel>();
            MySqlConnection connection = new MySqlConnection("server=localhost;port=8889;username=root;password=root;database=portfolio");
            connection.Open();
            string query = $"SELECT projects.id_project, projects.name_project, projects.image_path FROM projects;";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string id_project = reader.GetString(0);
                string name_project = reader.GetString(1);
                string image_path = reader.GetString(2);
                MyViewModel MyModel = new MyViewModel()
                {
                    Id = Convert.ToInt32(id_project),
                    Name = name_project,
                    Image = image_path
                };
                projects.Add(MyModel);
            }
            return View("Projects_login", projects);
        }
    }

    public IActionResult Sort_a_auth()
    {
        List<MyViewModel> projects = GetSortedProjects("ASC");
        return View("Projects_login", projects);
    }

    public IActionResult Sort_ya_auth()
    {
        List<MyViewModel> projects = GetSortedProjects("DESC");
        return View("Projects_login", projects);
    }

    [HttpPost]
    public IActionResult Search(string searchOption, string searchQuery)
    {
        if (searchOption == "option1")
        {
            List<MyViewModel> projects = new List<MyViewModel>();
            MySqlConnection connection = new MySqlConnection("server=localhost;port=8889;username=root;password=root;database=portfolio");
            connection.Open();
            string query = $"SELECT projects.id_project, projects.name_project, projects.image_path FROM projects WHERE projects.name_project LIKE '%{searchQuery}%';";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string id_project = reader.GetString(0);
                string name_project = reader.GetString(1);
                string image_path = reader.GetString(2);
                MyViewModel MyModel = new MyViewModel()
                {
                    Id = Convert.ToInt32(id_project),
                    Name = name_project,
                    Image = image_path
                };
                projects.Add(MyModel);
            }
            return View("Projects", projects);
        }
        else if (searchOption == "option2")
        {
            List<MyViewModel> projects = new List<MyViewModel>();
            MySqlConnection connection = new MySqlConnection("server=localhost;port=8889;username=root;password=root;database=portfolio");
            connection.Open();
            string query = $"SELECT projects.id_project, projects.name_project, projects.image_path FROM projects WHERE projects.skills LIKE '%{searchQuery}%';";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string id_project = reader.GetString(0);
                string name_project = reader.GetString(1);
                string image_path = reader.GetString(2);
                MyViewModel MyModel = new MyViewModel()
                {
                    Id = Convert.ToInt32(id_project),
                    Name = name_project,
                    Image = image_path
                };
                projects.Add(MyModel);
            }
            return View("Projects", projects);
        }
        else
        {
            List<MyViewModel> projects = new List<MyViewModel>();
            MySqlConnection connection = new MySqlConnection("server=localhost;port=8889;username=root;password=root;database=portfolio");
            connection.Open();
            string query = $"SELECT projects.id_project, projects.name_project, projects.image_path FROM projects;";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string id_project = reader.GetString(0);
                string name_project = reader.GetString(1);
                string image_path = reader.GetString(2);
                MyViewModel MyModel = new MyViewModel()
                {
                    Id = Convert.ToInt32(id_project),
                    Name = name_project,
                    Image = image_path
                };
                projects.Add(MyModel);
            }
            return View("Projects", projects);
        }
    }

    public IActionResult Sort_a()
    {
        List<MyViewModel> projects = GetSortedProjects("ASC");
        return View("Projects", projects);
    }

    private List<MyViewModel> GetSortedProjects(string sortOrder)
    {
        List<MyViewModel> projects = new List<MyViewModel>();
        MySqlConnection connection = new MySqlConnection("server=localhost;port=8889;username=root;password=root;database=portfolio");
        connection.Open();
        string query = $"select projects.id_project, projects.name_project, projects.image_path from projects ORDER BY name_project {sortOrder};";
        MySqlCommand command = new MySqlCommand(query, connection);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            string id_project = reader.GetString(0);
            string name_project = reader.GetString(1);
            string image_path = reader.GetString(2);
            MyViewModel MyModel = new MyViewModel()
            {
                Id = Convert.ToInt32(id_project),
                Name = name_project,
                Image = image_path
            };
            projects.Add(MyModel);
        }
        return projects;
    }

    public IActionResult Sort_ya()
    {
        List<MyViewModel> projects = GetSortedProjects("DESC");
        return View("Projects", projects);
    }

    //скачать CV
    [HttpPost]
    public IActionResult Index_login(string btn)
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;port=8889;username=root;password=root;database=portfolio");
        connection.Open();
        string query = $"SELECT users.email, users.name, users.profession, COUNT(projects.id_project) AS project_count FROM users" +
            $" JOIN projects ON users.email = projects.email WHERE users.email = '{userMail}' " +
            $" GROUP BY users.email, users.name, users.profession;";
        MySqlCommand command = new MySqlCommand(query, connection);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            string email = reader.GetString(0);
            string name = reader.GetString(1);
            string profession = reader.GetString(2);
            string kol_projects = reader.GetString(3);
            string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            if (!Directory.Exists(desktopPath))
            {
                Directory.CreateDirectory(desktopPath);
            }
            string filePath = Path.Combine(desktopPath, $"{userMail}_CV.txt");
            string newText = $"Имя: {name}\nПочта: {email}\nСпециальность: {profession}" +
                             $"\nКоличество проектов: {kol_projects}";
            try
            {
                using (StreamWriter sw = System.IO.File.CreateText(filePath))
                {
                    sw.Write(newText);
                }
            }
            catch (Exception ex)
            {
            }
        }
        return View();
    }

    [HttpPost]
    public IActionResult Projects(int id_project)
    {
        IdProjects = id_project;
        return RedirectToAction("Project");
    }

    [HttpPost]
    public IActionResult Projects_login(int id_project)
    {
        IdProjects = id_project;
        return RedirectToAction("Project_login");
    }

    [HttpPost]
    public IActionResult Kabinet(string profession, string name)
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;port=8889;username=root;password=root;database=portfolio");
        connection.Open();
        string query = $"UPDATE users SET profession = '{profession}' WHERE email='{userMail}'";
        MySqlCommand cmd = new MySqlCommand(query, connection);
        int rowsAffected = cmd.ExecuteNonQuery();
        if (rowsAffected > 0)
        {
            string mess = "Информация успешно обнавлена";
            ViewBag.Message = mess;
        }
        else
        {
            string mess = "Что-то пошло не так";
            ViewBag.Message = mess;
        }
        string query2 = $"UPDATE users SET name = '{name}' WHERE email='{userMail}'";
        MySqlCommand cmd2 = new MySqlCommand(query2, connection);
        int rowsAffected1 = cmd2.ExecuteNonQuery();
        if (rowsAffected1 > 0)
        {
            string mess = "Информация успешно обнавлена";
            ViewBag.Message = mess;
        }
        else
        {
            string mess = "Что-то пошло не так";
            ViewBag.Message = mess;
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Kabinet_add(string name_project, string skills, IFormFile image, string github)
    {
        ViewBag.ImagePath = await FileAdd(name_project, skills, image, github);
        return Redirect("Kabinet");
    }

    public async Task<string> FileAdd(string name_project, string skills, IFormFile image, string github)
    {
        string imageName = "/images/" + image.FileName;
        string fileName = imageName.Replace(" ", "");
        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + fileName, FileMode.Create))
        {
            await image.CopyToAsync(fileStream);
        }


        MySqlConnection connection = new MySqlConnection("server=localhost;port=8889;username=root;password=root;database=portfolio");
        connection.Open();
        string query = $"INSERT INTO `projects`(`id_project`, `email`, `name_project`, `skills`, `image_path`) VALUES (DEFAULT,'{userMail}','{name_project}','{skills}','{fileName}')";
        using (MySqlCommand command = new MySqlCommand(query, connection))
        {
            int row = command.ExecuteNonQuery();
            if (row > 0)
            {
                long id = command.LastInsertedId;
                string mess = "Данные успешно записаны!";
                ViewBag.Message = mess;
                string query2 = $"INSERT INTO `users_networks`(`id_users_networks`, `id_project`, `id_network`, `url`) VALUES (DEFAULT, '{id}','4','{github}')";
                using (MySqlCommand command2 = new MySqlCommand(query2, connection))
                {
                    int row2 = command2.ExecuteNonQuery();
                    if (row2 > 0)
                    {
                        string mess2 = "Данные успешно записаны!";
                        ViewBag.Message = mess2;
                    }
                    else
                    {
                        string mess2 = "Проверьте данные еще раз, возможно вы что-то ввели неправильно!";
                        ViewBag.Message = mess2;
                    }
                }
            }
            else
            {
                string mess = "Проверьте данные еще раз, возможно вы что-то ввели неправильно!";
                ViewBag.Message = mess;
            }
        }
        connection.Close();
        return imageName;
    }

    [HttpPost]
    public IActionResult Login(string email, string name, string pass, string repass)
    {
        string mas = "secsessful";
        ViewBag.Message = mas;
        MySqlConnection connection = new MySqlConnection("server=localhost;port=8889;username=root;password=root;database=portfolio");
        if (connection.State == System.Data.ConnectionState.Closed){
            connection.Open();
            string query = $"SELECT COUNT(*) FROM users WHERE email='{email}'"; 
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                int orgCount = Convert.ToInt32(command.ExecuteScalar());
                if (orgCount > 0)
                {
                    string mess = "Пользователь c таким ником уже есть!";
                    ViewBag.Message = mess;
                    return View();
                }
                else
                {
                    if (pass != repass)
                    {
                        string mess = "Пароли не совпадают!";
                        ViewBag.Message = mess;
                        return View();
                    }
                    else
                    {
                        string hashing = HashPassword(pass);
                        string query2 = $"INSERT INTO users(email, name, profession, password) VALUES('{email}', '{name}', DEFAULT, '{hashing}')";
                        MySqlCommand cmd = new MySqlCommand(query2, connection);
                        int i = cmd.ExecuteNonQuery();
                        if (i > -1)
                        {
                            string from = "aaaangeln01@mail.ru";
                            string to = email;
                            string password = "MPZm9kUpiSbHcCkhxghx";
                            MailMessage mailMessage = new MailMessage(from, to);
                            mailMessage.Subject = "YourPortfolio";
                            mailMessage.Body = $"Добро пожаловать на сайт YourPortfolio.\nСпасибо {email} за регистрацию , мы очень рады, что Вы теперь с нами!";
                            SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
                            smtp.Credentials = new System.Net.NetworkCredential(from, password);
                            smtp.EnableSsl = true;
                            smtp.Send(mailMessage);
                            string mess = "Вы успешно прошли регистрацию!";
                            ViewBag.Message = mess;
                            return Redirect("/Home/Auth");
                        }
                        else
                        {
                            string message2 = "Вы не зарегистрированы, попробуте ещё раз!";
                            ViewBag.Message = message2;
                            return View();
                        }
                    }
                }
            }
        }
        return View();
    }

    [HttpPost]
    public IActionResult Auth(string email, string pass)
    {
        if (email == "admin@admin.com")
        {
            if (pass == "adminadmin")
            {
                return Redirect("/Home/Admin");
            }
            else
            {
                string mess = "Пароль неверный!";
                ViewBag.Message = mess;
                return View();
            }
        }
        else
        {
            MySqlConnection connection = new MySqlConnection("server=localhost;port=8889;username=root;password=root;database=portfolio");
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
                string query = $"SELECT COUNT(*) FROM users WHERE email='{email}'";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    int orgCount = Convert.ToInt32(command.ExecuteScalar());
                    if (orgCount <= 0)
                    {
                        string mess = "Пользователя c таким ником нет, зарегистрируйтесь и попробуйте еще раз!";
                        ViewBag.Message = mess;
                        return View();
                    }
                    else
                    {
                        string hashPassword = HashPassword(pass);
                        string query2 = $"select count(*) from users where email='{email}' and password='{hashPassword}'";
                        MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                        int count = Convert.ToInt32(cmd2.ExecuteScalar());
                        if (count == 0)
                        {
                            string mess = "Пароль неверный!";
                            ViewBag.Message = mess;
                            return View();
                        }
                        else
                        {
                            userMail = email;
                            connection.Close();
                            return Redirect("/Home/Index_login");
                        }
                    }
                }
            }
        }
        return View();
    }
    
    public string HashPassword(string pass)
    {
        SHA256 hash = SHA256.Create();
        byte[] bytes = Encoding.UTF8.GetBytes(pass);
        byte[] password = hash.ComputeHash(bytes);
        string hashPassword = Convert.ToBase64String(password);
        return hashPassword;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

