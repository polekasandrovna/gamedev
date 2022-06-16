using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lab1.Models;

namespace Lab1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// (Контроллер)
    /// </summary>
    public partial class MainWindow : Window
    {

        GameFieldModel model = new GameFieldModel();

        public MainWindow()
        {
            InitializeComponent();

            prepareBeforeStart();

        }

        //Добавление объекта на главное окно
        public void addObjectToMainField(UIElement obj, int column, int row, int columnSpan = 1, int rowSpan = 1)
        {
            field.Children.Add(obj);
            Grid.SetColumn(obj, column);
            Grid.SetRow(obj, row);
            Grid.SetColumnSpan(obj, columnSpan);
            Grid.SetRowSpan(obj, rowSpan);
        }

        //Подготовка перед началом игры
        private void prepareBeforeStart()
        {

            initialisenGameFieldOnMainWindow();

            model.playAgainButton.Click += PlayAgainButton_Click;

            addObjectToMainField(model.playAgainButton, 1, 10);

            model.newGameField();

            addObjectToMainField(model.winTB, 5, 7, 13, 6);
            addObjectToMainField(model.countOfWallsPlayer1, 1, 7);
            addObjectToMainField(model.countOfWallsPlayer2, 1, 13);

            model.opponentButton = btnSettings;
        }

        //Добавление игрового поля на главное окно
        private void initialisenGameFieldOnMainWindow()
        {
            int id = 0;
            int i = 3, j = 2;
            int maxField = i + 17;
            foreach (var item in model.floorButtons)
            {
                item.Click += ButtonFloor_Click;
                item.Name = "i" + id.ToString();
                addObjectToMainField(item, i, j);
                id++;
                i += 2;
                if (i >= maxField)
                {
                    i = maxField - 17;
                    j += 2;
                }
            }

            id = 0;
            i = 4;
            j = 2;
            maxField = i + 16;
            foreach (var item in model.wallButtons)
            {
                item.Click += ButtonWall_Click;
                item.Name = "w" + id.ToString();
                addObjectToMainField(item, i, j);
                id++;
                i += 2;
                if (i >= maxField)
                {
                    j++;
                    if (j % 2 == 0)
                    {
                        i = maxField - 16;
                    }
                    else
                    {
                        i = maxField - 17;
                    }
                }
            }

            id = 0;
            i = 4;
            j = 3;
            maxField = i + 16;
            foreach (var item in model.pillarButtons)
            {
                item.Click += ButtonPillar_Click;
                item.Name = "p" + id.ToString();
                addObjectToMainField(item, i, j);
                id++;
                i += 2;
                if (i >= maxField)
                {
                    j += 2;
                    i = maxField - 16;
                }
            }
        }

        //Обработчик нажатия на кнопку start again в меню
        private void MenuItemStart_Click(object sender, RoutedEventArgs e)
        {
            model.newGameField();
        }

        //Обработчик нажатия на плитку для хода игрока
        private void ButtonFloor_Click(object sender, RoutedEventArgs e)
        {
            model.playerTurn((Button)sender);
        }

        //Обработчик нажатия на стену
        private void ButtonWall_Click(object sender, RoutedEventArgs e)
        {
            model.placeSelectedWalls((Button)sender);
        }

        //Обработчик нажатия на колонну
        private void ButtonPillar_Click(object sender, RoutedEventArgs e)
        {
            model.selectPillar((Button)sender);
        }

        //Обработчик нажатия на кнопку play again
        private void PlayAgainButton_Click(object sender, RoutedEventArgs e)
        {
            model.newGameField();
        }

        //Обработчик нажатия на кнопку "Opponent: AI"/"Opponent: Player" в меню
        private void MenuItemOpponent_Click(object sender, RoutedEventArgs e)
        {
            model.changeOpponent();

        }

        //Обработчик нажатия на кнопку Quit в меню
        private void MenuItemQuit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
