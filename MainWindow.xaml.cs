using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FiltrData
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            using (MarathonEntities db = new MarathonEntities())
            {
                db.Registration.Load();
                db.User.Load();
                db.Runner.Load();
                db.Role.Load();

                cbxRole.ItemsSource = db.Role.Local.ToBindingList();

                var query = from u in db.Registration
                            select new { u.Runner.User.LastName, u.Runner.User.FirstName, u.Runner.User.Email, u.Runner.User.Role.RoleName };

                dgUsers.ItemsSource = query.ToList();
                tblCount.Text = query.Count().ToString();

                //dgUsers.ItemsSource = query.ToList();
            }
        }

        private void Filter(object sender, RoutedEventArgs e)
        {
            Role selectedRole = (Role)cbxRole.SelectedItem;

            using (MarathonEntities db = new MarathonEntities())
            {
                try
                {
                    switch (selectedRole.RoleName)
                    {
                        case "Administrator":
                            var queryAdmin = from u in db.Registration
                                             where (u.Runner.User.RoleId.ToString() == selectedRole.RoleId)
                                             select new { u.Runner.User.LastName, u.Runner.User.FirstName, u.Runner.User.Email, u.Runner.User.Role.RoleName };
                            dgUsers.ItemsSource = queryAdmin.ToList();
                            break;

                        case "Runner":
                            var queryRunner = from u in db.Registration
                                              where (u.Runner.User.RoleId.ToString() == selectedRole.RoleId)
                                              select new { u.Runner.User.LastName, u.Runner.User.FirstName, u.Runner.User.Email, u.Runner.User.Role.RoleName };
                            dgUsers.ItemsSource = queryRunner.ToList();
                            break;

                        case "Coordinator":
                            var queryCoordinator = from u in db.Registration
                                                   where (u.Runner.User.RoleId.ToString() == selectedRole.RoleId)
                                                   select new { u.Runner.User.LastName, u.Runner.User.FirstName, u.Runner.User.Email, u.Runner.User.Role.RoleName };
                            dgUsers.ItemsSource = queryCoordinator.ToList();
                            break;
                    }
                }
                catch { }

                try
                {
                    switch (cbxSort.Text)
                    {
                        case "Фамилии":
                            dgUsers.Items.SortDescriptions.Add(new SortDescription("Фамилия", ListSortDirection.Descending));
                            dgUsers.Items.Refresh();
                            break;

                        case "Имени":
                            dgUsers.Items.SortDescriptions.Add(new SortDescription("Имя", ListSortDirection.Descending));
                            dgUsers.Items.Refresh();
                            break;

                        case "Email":
                            dgUsers.Items.SortDescriptions.Add(new SortDescription("Email", ListSortDirection.Descending));
                            dgUsers.Items.Refresh();
                            break;

                        case "Роли":
                            dgUsers.Items.SortDescriptions.Add(new SortDescription("Роль", ListSortDirection.Descending));
                            dgUsers.Items.Refresh();
                            break;
                    }
                }
                catch { }


            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            using (MarathonEntities db = new MarathonEntities())
            {
                var search = from u in db.User
                             where (u.FirstName.Contains(tbxSearch.Text) || (u.LastName.Contains(tbxSearch.Text))
                             || (u.Email.Contains(tbxSearch.Text)) || (u.Role.RoleName.Contains(tbxSearch.Text)))
                             select new { u.LastName, u.FirstName, u.Email, u.Role.RoleName };

                dgUsers.ItemsSource = search.ToList();
            }
        }
    }
}
