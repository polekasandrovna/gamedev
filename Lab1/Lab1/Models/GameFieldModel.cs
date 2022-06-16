using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Lab1.Models
{
    public class GameFieldModel
    {
        //Список клеток по которым перемещаются игроки
        public List<Button> floorButtons { get; set; }
        //Список клеток со стенами
        public List<Button> wallButtons { get; set; }
        //Список клеток с колоннами
        public List<Button> pillarButtons { get; set; }
        //Список всех клеток в виде игрового поля
        public List<Button> allButtons { get; set; }

        //Является ли оппонент AI? Если false - оппонент второй игрок
        private bool isOpponentAI = false;

        //Генерация случайных чисел
        private Random rnd = new Random();

        //Объекты изображений
        private ImageBrush player1Img = new ImageBrush();
        private ImageBrush player2Img = new ImageBrush();
        private ImageBrush floorImg = new ImageBrush();
        private ImageBrush groundImg = new ImageBrush();
        private ImageBrush wallImg = new ImageBrush();

        //Игрок 1
        private Button player1;

        //Игрок 2
        private Button player2;

        //Игрой, чей ход наступил
        private Button currentPlayer;

        //Выбранная колонна
        private Button currentPillar;

        //Кнопка начала новой игры
        public Button playAgainButton { get; set; }

        //Кнопка меню для изменения режима оппонента
        public MenuItem opponentButton { get; set; }

        //Текстовое поле, в котором объявляется победитель
        public TextBlock winTB { get; set; }

        //Текстовое поле, содержащее информацию о количестве доступных стен у игрока 1
        public TextBlock countOfWallsPlayer1 { get; set; }
        //Текстовое поле, содержащее информацию о количестве доступных стен у игрока 2
        public TextBlock countOfWallsPlayer2 { get; set; }

        //Конструктор
        public GameFieldModel()
        {
            setImages();
            initialiseGameField();
        }


        //Обьявление переменных и заполнение списков
        private void initialiseGameField()
        {
            playAgainButton = new Button();
            countOfWallsPlayer1 = new TextBlock();
            countOfWallsPlayer2 = new TextBlock();
            winTB = new TextBlock();

            floorButtons = new List<Button>();
            for (int i = 0; i < 81; i++)
            {
                floorButtons.Add(new Button());

            }

            wallButtons = new List<Button>();
            for (int i = 0; i < 144; i++)
            {
                wallButtons.Add(new Button());

            }
            pillarButtons = new List<Button>();
            for (int i = 0; i < 64; i++)
            {
                pillarButtons.Add(new Button());
            }


            int fb = 0, wb = 0, pb = 0;
            allButtons = new List<Button>();
            for (int i = 0; i < 17; i++)
            {
                for (int j = 0; j < 17; j++)
                {
                    if (i % 2 == 0)
                    {
                        if (j % 2 == 0)
                        {
                            allButtons.Add(floorButtons[fb]);
                            fb++;
                        }
                        else
                        {
                            allButtons.Add(wallButtons[wb]);
                            wb++;
                        }
                    }
                    else
                    {
                        if (j % 2 == 0)
                        {
                            allButtons.Add(wallButtons[wb]);
                            wb++;
                        }
                        else
                        {
                            allButtons.Add(pillarButtons[pb]);
                            pb++;
                        }
                    }
                }
            }
        }

        //Определение изображений графических элементов
        private void setImages()
        {
            player1Img.ImageSource = new BitmapImage(new Uri("Res/player1.png", UriKind.Relative));
            player2Img.ImageSource = new BitmapImage(new Uri("Res/player2.png", UriKind.Relative));
            floorImg.ImageSource = new BitmapImage(new Uri("Res/gamebg.jpg", UriKind.Relative));
            groundImg.ImageSource = new BitmapImage(new Uri("Res/ground.png", UriKind.Relative));
            wallImg.ImageSource = new BitmapImage(new Uri("Res/wall.jpg", UriKind.Relative));
        }

        //Алгоритм принятия решений AI
        private bool makeAIDecision()
        {
            if (!isOpponentAI)
            {
                return false;
            }
            int secondRandomBorder = 2;
            if (Convert.ToInt32(countOfWallsPlayer2.Text[16].ToString()) <= 0)
            {
                secondRandomBorder = 1;
            }

            if (rnd.Next(0, secondRandomBorder) == 0)
            {
                int playerIndex = allButtons.FindIndex(x => x.Name == currentPlayer.Name);
                Button stepButton = new Button();
                do
                {
                    try
                    {
                        switch (rnd.Next(0, 4))
                        {
                            case 0:
                                stepButton = allButtons[playerIndex + 2];
                                break;
                            case 1:
                                stepButton = allButtons[playerIndex - 2];
                                break;
                            case 2:
                                stepButton = allButtons[playerIndex + 34];
                                break;
                            case 3:
                                stepButton = allButtons[playerIndex - 34];
                                break;
                        }
                    }
                    catch (Exception)
                    {
                        stepButton = null;
                    }

                } while (!AIPlayerTurn(stepButton));
            }
            else
            {

                Button selectedWall = new Button();
                do
                {
                    currentPillar = pillarButtons[rnd.Next(0, pillarButtons.Count - 1)];
                    int pillarIndex = allButtons.FindIndex(x => x.Name == currentPillar.Name);
                    try
                    {
                        switch (rnd.Next(0, 4))
                        {
                            case 0:
                                if (allButtons[pillarIndex + 1].Background != wallImg
                                && allButtons[pillarIndex - 1].Background != wallImg
                                && (allButtons[pillarIndex + 17].Background == groundImg
                                || allButtons[pillarIndex - 17].Background == groundImg))
                                {
                                    selectedWall = allButtons[pillarIndex + 1];
                                }
                                break;
                            case 1:
                                if (allButtons[pillarIndex + 1].Background != wallImg
                                && allButtons[pillarIndex - 1].Background != wallImg
                                && (allButtons[pillarIndex + 17].Background == groundImg
                                || allButtons[pillarIndex - 17].Background == groundImg))
                                {
                                    selectedWall = allButtons[pillarIndex - 1];
                                }
                                break;
                            case 2:
                                if (allButtons[pillarIndex - 17].Background != wallImg
                                && allButtons[pillarIndex + 17].Background != wallImg
                                && (allButtons[pillarIndex - 1].Background == groundImg
                                || allButtons[pillarIndex + 1].Background == groundImg))
                                {
                                    selectedWall = allButtons[pillarIndex + 17];
                                }
                                break;
                            case 3:
                                if (allButtons[pillarIndex - 17].Background != wallImg
                                && allButtons[pillarIndex + 17].Background != wallImg
                                && (allButtons[pillarIndex - 1].Background == groundImg
                                || allButtons[pillarIndex + 1].Background == groundImg))
                                {
                                    selectedWall = allButtons[pillarIndex - 17];
                                }
                                break;
                        }
                    }
                    catch (Exception){}
                    
                    placeSelectedWalls(selectedWall);
                } while (selectedWall.Background != wallImg);

            }
            return true;
        }

        //Отключение всех активных клеток на игровом поле
        private void disableGameField()
        {
            foreach (var item in allButtons)
            {
                item.IsHitTestVisible = false;
                item.BorderBrush = Brushes.Gray;
            }
        }

        //Включение клеток по оси X и Y вокруг выбранной на указанном расстоянии
        //centerButton - клетка, вокруг которой нужно включить соседние
        //countCells - радиус круга, измеряется количеством клеток
        //checkWalls - нужно ли проверять соседние клетки на наличие стен
        //checkAbleWalls - проверка на доступность размещения стен
        private void enableButtonsAround(Button centerButton, int countCells, bool checkWalls, bool checkAbleWalls)
        {
            int playerIndex = allButtons.FindIndex(x => x.Name == centerButton.Name);
            int horizontal = 17 * countCells;
            int vertical = 1 * countCells;

            if (!checkAbleWalls || allButtons[playerIndex - vertical].Background != wallImg
                            && allButtons[playerIndex + vertical].Background != wallImg 
                            && (allButtons[playerIndex - horizontal].Background == groundImg
                            || allButtons[playerIndex + horizontal].Background == groundImg))
            {
                try
                {
                    if ((playerIndex - vertical) / 17 == playerIndex / 17)
                    {
                        if (!checkWalls || allButtons[playerIndex - vertical + 1].Background != wallImg)
                        {
                            allButtons[playerIndex - vertical].IsHitTestVisible = true;
                            allButtons[playerIndex - vertical].BorderBrush = Brushes.LightCyan;
                        }
                    }
                }

            catch (Exception) { }

            try
            {
                if ((playerIndex + vertical) / 17 == playerIndex / 17)
                {
                    if (!checkWalls || allButtons[playerIndex + vertical - 1].Background != wallImg)
                    {
                            allButtons[playerIndex + vertical].IsHitTestVisible = true;
                            allButtons[playerIndex + vertical].BorderBrush = Brushes.LightCyan;
                    }
                }

            }
            catch (Exception) { }
        }

            if (!checkAbleWalls || allButtons[playerIndex - horizontal].Background != wallImg
                            && allButtons[playerIndex + horizontal].Background != wallImg
                            && (allButtons[playerIndex - vertical].Background == groundImg
                            || allButtons[playerIndex + vertical].Background == groundImg))
            {

                try
                {
                    if (!checkWalls || allButtons[playerIndex - horizontal + 17].Background != wallImg)
                    {
                            allButtons[playerIndex - horizontal].IsHitTestVisible = true;
                            allButtons[playerIndex - horizontal].BorderBrush = Brushes.LightCyan;
                    }
                }
                catch (Exception) { }

                try
                {
                    if (!checkWalls || allButtons[playerIndex + horizontal - 17].Background != wallImg)
                    {
                            allButtons[playerIndex + horizontal].IsHitTestVisible = true;
                            allButtons[playerIndex + horizontal].BorderBrush = Brushes.LightCyan;
                    }
                }
                catch (Exception) { }
            }
        }

        //Алгоритм проверки доступных ходов для определённого игрока
        //player - игрок, для которого проверяется доступность ходов
        private bool checkAvaibleTurns(Button player)
        {

            List<Button> buttonQueue = new List<Button>();
            List<Button> viewedButtons = new List<Button>();

            int objectIndex = 0;
            int floorIndex = 0;
            int horizontal = 34;
            int vertical = 2;

            buttonQueue.Add(player);

            for (int i = 0; i < buttonQueue.Count; i++)
            {
                viewedButtons.Add(buttonQueue[i]);
                objectIndex = allButtons.FindIndex(x => x.Name == buttonQueue[i].Name);
                floorIndex = floorButtons.FindIndex(x => x.Name == buttonQueue[i].Name);

                if ((player1.Name == player.Name && floorIndex <= 80 && floorIndex >= 72)
                    || (player2.Name == player.Name && floorIndex <= 8 && floorIndex >= 0))
                {
                    return true;
                }

                objectIndex = allButtons.FindIndex(x => x.Name == buttonQueue[i].Name);
                try
                {
                    if ((objectIndex - vertical) / 17 == objectIndex / 17
                        && allButtons[objectIndex - vertical + 1].Background != wallImg
                        && viewedButtons.FindIndex(x => x.Name == allButtons[objectIndex - vertical].Name) == -1)
                        {
                            buttonQueue.Add(allButtons[objectIndex - vertical]);
                        
                    }
                }

                catch (Exception) { }

                try
                {
                    if ((objectIndex + vertical) / 17 == objectIndex / 17 
                        && allButtons[objectIndex + vertical - 1].Background != wallImg
                        && viewedButtons.FindIndex(x => x.Name == allButtons[objectIndex + vertical].Name) == -1)
                        {
                            buttonQueue.Add(allButtons[objectIndex + vertical]);
                    }

                }
                catch (Exception) { }


                try
                {
                    if (allButtons[objectIndex - horizontal + 17].Background != wallImg
                        && viewedButtons.FindIndex(x => x.Name == allButtons[objectIndex - horizontal].Name) == -1)
                    {
                        buttonQueue.Add(allButtons[objectIndex - horizontal]);

                    }
                }
                catch (Exception) { }

                try
                {
                    if (allButtons[objectIndex + horizontal - 17].Background != wallImg
                        && viewedButtons.FindIndex(x => x.Name == allButtons[objectIndex + horizontal].Name) == -1)
                    {
                        buttonQueue.Add(allButtons[objectIndex + horizontal]);
                    }
                }
                catch (Exception) { }
            }

            return false;
        }

        //Изменение оппонента (AI/второй игрок)
        public void changeOpponent()
        {
            if (isOpponentAI)
            {
                opponentButton.Header = "Opponent: Player";
            }
            else
            {
                opponentButton.Header = "Opponent: AI";
            }
            isOpponentAI = !isOpponentAI;
        }

        //Подготовка к следующему ходу
        private void makeStep()
        {
            disableGameField();

            if (currentPlayer.Name == player2.Name)
            {
                if (makeAIDecision())
                {
                    return;
                }
            }
            

            try
            {
                if (currentPlayer.Name == player1.Name && Convert.ToInt32(countOfWallsPlayer1.Text[16].ToString()) > 0
                || currentPlayer.Name == player2.Name && Convert.ToInt32(countOfWallsPlayer2.Text[16].ToString()) > 0)
                {
                    foreach (var item in pillarButtons)
                    {
                        item.BorderBrush = Brushes.LightCyan;
                        item.IsHitTestVisible = true;
                    }
                }
            }
            catch (Exception){}
            

            enableButtonsAround(currentPlayer, 2, true, false);
        }

        //Проверка колонны на наличие стен вокруг
        //pillar - колонна, вокруг которой нужно проверить наличие стен
        private bool checkWallsAround(Button pillar)
        {
            int pillarIndex = allButtons.FindIndex(x => x.Name == pillar.Name);

            if (allButtons[pillarIndex + 1].Background == wallImg 
                || allButtons[pillarIndex - 1].Background == wallImg
                || allButtons[pillarIndex + 17].Background == wallImg
                || allButtons[pillarIndex - 17].Background == wallImg)
            {
                return true;
            }
            return false;
        }

        //Изменение количества стен у текущего игрока
        //countOfWalls - количество стен, которое нужно прибавить
        private void addWallsToCurrentPlayer(int countOfWalls)
        {
            try
            {
                if (currentPlayer.Name == player1.Name)
                {
                    countOfWallsPlayer1.Text = countOfWallsPlayer1.Text.Replace("walls: " + countOfWallsPlayer1.Text[16], "walls: " + (Convert.ToInt32(countOfWallsPlayer1.Text[16].ToString()) + countOfWalls).ToString());
                }
                else if (currentPlayer.Name == player2.Name)
                {
                    countOfWallsPlayer2.Text = countOfWallsPlayer2.Text.Replace("walls: " + countOfWallsPlayer2.Text[16], "walls: " + (Convert.ToInt32(countOfWallsPlayer2.Text[16].ToString()) + countOfWalls).ToString());
                }
            }
            catch (Exception) { }
        }

        //Поставить нажатую стену и ту, которая находится
        //с противоположной стороны от выбранной колонны
        //clickedWall - нажатая стена
        public void placeSelectedWalls(Button clickedWall)
        {
            if (clickedWall.Name == "")
            {
                return;
            }

            addWallsToCurrentPlayer(-1);
            
                clickedWall.Background = wallImg;
            int currentPillarIndex = allButtons.FindIndex(x => x.Name == currentPillar.Name);
            int clickedWallIndex = allButtons.FindIndex(x => x.Name == clickedWall.Name);
            clickedWall.Background = wallImg;
            currentPillar.Background = wallImg;
            try
            {
                if (currentPillarIndex + 1 == clickedWallIndex)
                {
                    allButtons[currentPillarIndex - 1].Background = wallImg;
                    if ((currentPillarIndex + 2) / 17 == currentPillarIndex / 17 )
                    {
                        allButtons[currentPillarIndex + 2].Background = wallImg;
                    }
                    if ((currentPillarIndex - 2) / 17 == currentPillarIndex / 17)
                    {
                        allButtons[currentPillarIndex - 2].Background = wallImg;
                    }

                    if (!checkAvaibleTurns(player1) || !checkAvaibleTurns(player2))
                    {
                        addWallsToCurrentPlayer(1);
                        clickedWall.Background = groundImg;
                        allButtons[currentPillarIndex - 1].Background = groundImg;
                        if (!checkWallsAround(allButtons[currentPillarIndex + 2]) 
                            && (currentPillarIndex + 2) / 17 == currentPillarIndex / 17)
                        {
                            allButtons[currentPillarIndex + 2].Background = groundImg;
                        }
                        if (!checkWallsAround(allButtons[currentPillarIndex - 2])
                            && (currentPillarIndex - 2) / 17 == currentPillarIndex / 17)
                        {
                            allButtons[currentPillarIndex - 2].Background = groundImg;
                        }
                        if (!checkWallsAround(currentPillar))
                        {
                            currentPillar.Background = groundImg;
                        }
                        return;
                    }
                }
                else if (currentPillarIndex - 1 == clickedWallIndex)
                {
                    allButtons[currentPillarIndex + 1].Background = wallImg;
                    if ((currentPillarIndex + 2) / 17 == currentPillarIndex / 17)
                    {
                        allButtons[currentPillarIndex + 2].Background = wallImg;
                    }
                    if ((currentPillarIndex - 2) / 17 == currentPillarIndex / 17)
                    {
                        allButtons[currentPillarIndex - 2].Background = wallImg;
                    }
                    if (!checkAvaibleTurns(player1) || !checkAvaibleTurns(player2))
                    {
                        addWallsToCurrentPlayer(1);
                        clickedWall.Background = groundImg;
                        allButtons[currentPillarIndex + 1].Background = groundImg;
                        if (!checkWallsAround(allButtons[currentPillarIndex + 2])
                            && (currentPillarIndex + 2) / 17 == currentPillarIndex / 17)
                        {
                            allButtons[currentPillarIndex + 2].Background = groundImg;
                        }
                        if (!checkWallsAround(allButtons[currentPillarIndex - 2])
                            && (currentPillarIndex - 2) / 17 == currentPillarIndex / 17)
                        {
                            allButtons[currentPillarIndex - 2].Background = groundImg;
                        }
                        if (!checkWallsAround(currentPillar))
                        {
                            currentPillar.Background = groundImg;
                        }
                        return;
                    }
                }
                else if (currentPillarIndex - 17 == clickedWallIndex)
                {
                        allButtons[currentPillarIndex + 17].Background = wallImg;
                    try
                    {
                        allButtons[currentPillarIndex - 34].Background = wallImg;
                    }
                    catch (Exception){}
                    try
                    {
                        allButtons[currentPillarIndex + 34].Background = wallImg;
                    }
                    catch (Exception){}

                    if (!checkAvaibleTurns(player1) || !checkAvaibleTurns(player2))
                    {
                        addWallsToCurrentPlayer(1);
                        clickedWall.Background = groundImg;
                        allButtons[currentPillarIndex + 17].Background = groundImg;
                        try
                        {
                            if (!checkWallsAround(allButtons[currentPillarIndex + 34]))
                            {
                                allButtons[currentPillarIndex + 34].Background = groundImg;
                            }
                        }
                        catch (Exception){}
                        try
                        {
                            if (!checkWallsAround(allButtons[currentPillarIndex - 34]))
                            {
                                allButtons[currentPillarIndex - 34].Background = groundImg;
                            }
                        }
                        catch (Exception){}

                        if (!checkWallsAround(currentPillar))
                        {
                            currentPillar.Background = groundImg;
                        }
                        return;
                    }
                }
                else if (currentPillarIndex + 17 == clickedWallIndex)
                {
                    allButtons[currentPillarIndex - 17].Background = wallImg;
                    try
                    {
                        allButtons[currentPillarIndex - 34].Background = wallImg;
                    }
                    catch (Exception) { }
                    try
                    {
                        allButtons[currentPillarIndex + 34].Background = wallImg;
                    }
                    catch (Exception) { }
                    if (!checkAvaibleTurns(player1) || !checkAvaibleTurns(player2))
                    {
                        addWallsToCurrentPlayer(1);
                        clickedWall.Background = groundImg;
                        allButtons[currentPillarIndex - 17].Background = groundImg;
                        try
                        {
                            if (!checkWallsAround(allButtons[currentPillarIndex + 34]))
                            {
                                allButtons[currentPillarIndex + 34].Background = groundImg;
                            }
                        }
                        catch (Exception) { }
                        try
                        {
                            if (!checkWallsAround(allButtons[currentPillarIndex - 34]))
                            {
                                allButtons[currentPillarIndex - 34].Background = groundImg;
                            }
                        }
                        catch (Exception) { }
                        if (!checkWallsAround(currentPillar))
                        {
                            currentPillar.Background = groundImg;
                        }
                        return;
                    }
                }
            }
            catch (Exception)
            {
            }
            
            if (player1.Name == currentPlayer.Name)
            {
                currentPlayer = player2;
            }
            else
            {
                currentPlayer = player1;
            }
            makeStep();
        }

        //Показывает варианты ходов с выбранной колонной
        //pillar - выбранная колонна
        public void selectPillar(Button pillar)
        {
            currentPillar = pillar;
            disableGameField();
            Button selectedButton = allButtons.Find(x => x.Name == pillar.Name);
            enableButtonsAround(selectedButton, 1, false, true);
            enableButtonsAround(selectedButton, 2, false, false);
            enableButtonsAround(currentPlayer, 2, true, false);
        }

        //Проверки для перемещения игрока на выбранную клетку во время игры с AI
        //stepButton - клетка, которую нужно проверить на доступность перемещения
        private bool AIPlayerTurn(Button stepButton)
        {

            if (stepButton == null)
            {
                return false;
            }

            int objectIndex = allButtons.FindIndex(x => x.Name == stepButton.Name);
            int playerIndex = allButtons.FindIndex(x => x.Name == currentPlayer.Name);
            if ((objectIndex / 17 == playerIndex / 17 || objectIndex % 34 == playerIndex % 34)
                && allButtons[playerIndex + ((objectIndex - playerIndex) / 2)].Background == groundImg)
            {
                try
                {
                    playerTurn(stepButton);
                    return true;
                }
                catch (Exception){}
            }

            return false;
        }

        //Перемещение игрока на указанную клетку
        //stepButton - клетка, на которую перемещается игрок
        public void playerTurn(Button stepButton)
        {
            int index = floorButtons.FindIndex(x => x.Name == stepButton.Name);
            if (player1.Name == currentPlayer.Name)
            {
                player1.Background = floorImg;
                player1 = stepButton;
                player1.Background = player1Img;
                currentPlayer = player2;
                if (index <= 80 && index >= 72)
                {
                    winTB.IsEnabled = true;
                    winTB.Text = "Player 1 wins!";

                    disableGameField();

                    playAgainButton.Visibility = Visibility.Visible;
                    winTB.Visibility = Visibility.Visible;
                    return;
                }
            }
            else
            {
                player2.Background = floorImg;
                player2 = stepButton;
                player2.Background = player2Img;
                currentPlayer = player1;
                if (index <= 8 && index >= 0)
                {
                    winTB.IsEnabled = true;
                    winTB.Text = "Player 2 wins!";

                    disableGameField();

                    playAgainButton.Visibility = Visibility.Visible;
                    winTB.Visibility = Visibility.Visible;
                    return;
                }
            }
            makeStep();
        }

        //Разрисовка игрового поля изображениями
        private void setFloorButtons(int i, int j)
        {
            foreach (var item in floorButtons)
            {
                item.Background = floorImg;
                item.IsHitTestVisible = false;
            }
            foreach (var item in wallButtons)
            {
                item.Background = groundImg;
                item.IsHitTestVisible = false;
            }
            foreach (var item in pillarButtons)
            {
                item.Background = groundImg;
            }
            makeStep();
        }

        //Подготовка поля к новой игре
        public void newGameField()
        {
            player1 = floorButtons[4];
            player2 = floorButtons[76];

            countOfWallsPlayer1.Text = "Player 1 walls: 5";
            countOfWallsPlayer1.HorizontalAlignment = HorizontalAlignment.Center;
            countOfWallsPlayer1.VerticalAlignment = VerticalAlignment.Center;
            countOfWallsPlayer2.Text = "Player 2 walls: 5";
            countOfWallsPlayer2.HorizontalAlignment = HorizontalAlignment.Center;
            countOfWallsPlayer2.VerticalAlignment = VerticalAlignment.Center;

            playAgainButton.Visibility = Visibility.Collapsed;
            playAgainButton.Content = "Play again!";

            winTB.FontSize = 77;
            winTB.Visibility = Visibility.Collapsed;
            winTB.HorizontalAlignment = HorizontalAlignment.Center;
            winTB.VerticalAlignment = VerticalAlignment.Center;

            currentPlayer = player1;

            setFloorButtons(3, 2);

            player1.Background = player1Img;
            player2.Background = player2Img;
        }
    }
}
