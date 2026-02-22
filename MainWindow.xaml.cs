using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tetris;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{

    private readonly ImageSource[] tileImages = new ImageSource[]
    {
        new BitmapImage(new Uri("Assets/TileEmpty.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/TileCyan.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/TileBlue.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/TileOrange.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/TileYellow.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/TileGreen.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/TilePurple.png", UriKind.Relative)),
         new BitmapImage(new Uri("Assets/TileRed.png", UriKind.Relative))
    };


    private readonly ImageSource[] blockImages = new ImageSource[]
   {
        new BitmapImage(new Uri("Assets/Block-Empty.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Block-I.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Block-J.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Block-L.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Block-O.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Block-S.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Block-T.png", UriKind.Relative)),
        new BitmapImage(new Uri("Assets/Block-Z.png", UriKind.Relative)),
   };

    //2-d array of image controls - 1 image control for every cell
    private readonly Image[,] imageControls;
    private readonly int maxDelay = 1000;
    private readonly int minDelay = 75;
    private readonly int delayDecrease = 25;

    private GameState gameState = new GameState();


    public MainWindow()
    {
        InitializeComponent();
        this.Focusable = true;
        this.Focus();
        //initialize imageControls array
        imageControls = setupGameCanvas(gameState.GameGrid);
    }
    

    private Image[,] setupGameCanvas(GameGrid grid)
    {
        //setup image controls
        //array has 22 rows and 10 cols just like game grid
        Image[,] imageControls = new Image[grid.rows, grid.columns];
        int cellSize = 25;

        //loop through every row and col in game grid
        for(int r = 0; r < grid.rows; r++)
        {
            for(int c=0; c<grid.columns; c++)
            {
                //for each position we create a new imagecontrol of 25px x 25px
                Image imageControl = new Image
                {
                    Width = cellSize,
                    Height = cellSize
                };

                //position image control correctly
                //-2 pushes top hidden rows up so they are not visible on canvas
                Canvas.SetTop(imageControl, (r - 2) * cellSize+10);
                Canvas.SetLeft(imageControl, c * cellSize);

                //make image child of canvas and add to array for return outside loop
                GameCanvas.Children.Add(imageControl);
                imageControls[r, c] = imageControl;

            }
        }

        return imageControls;
    }

    private void drawGrid(GameGrid grid)
    {
        //loop through all positions 
        for(int r = 0; r < grid.rows; r++)
        {
            for(int c = 0; c < grid.columns; c++)
            {
                //for each position get stored ID
                int id = grid.getValue(r,c);
                imageControls[r, c].Opacity = 1;
                //assign the source of the image at this position using this id
                imageControls[r, c].Source = tileImages[id];
            }
        }
    }

    private void drawBlock(Block block)
    {
        //loop through tile positions and update the image sources
        foreach (Position p in block.tilePositions())
        {
            if (p.row >= 0 && p.row < gameState.GameGrid.getRows() &&
                p.column >= 0 && p.column < gameState.GameGrid.getColumns())
            {
                imageControls[p.row, p.column].Opacity = 1;
                imageControls[p.row, p.column].Source = tileImages[block.ID];
            }
        }
    }

    private void drawNextBlock(BlockQueue blockQueue)
    {
        Block next = blockQueue.nextBlock;
        nextImage.Source = blockImages[next.ID];
    }

    private void drawHeldBlock(Block heldBlock)
    {
        if (heldBlock == null)
        {
            holdImage.Source = blockImages[0];
        }
        else
        {
            holdImage.Source = blockImages[heldBlock.ID];
        }
    }
    
    private void drawGhostBlock(Block block)
    {
        int dropDistance = gameState.blockDropDistance();

        foreach(Position p in block.tilePositions())
        {
            imageControls[p.row + dropDistance, p.column].Opacity = 0.25;
            imageControls[p.row + dropDistance, p.column].Source = tileImages[block.ID];
        }

    }
    private void draw(GameState gameState)
    {
       
        drawGrid(gameState.GameGrid);
        drawGhostBlock(gameState.CurrentBlock);
        drawBlock(gameState.CurrentBlock);
        drawNextBlock(gameState.BlockQueue);
        drawHeldBlock(gameState.HeldBlock);
        ScoreText.Text = $"Score: {gameState.Score}";

    }

    private async Task gameLoop()
    {
        draw(gameState);

        while (!gameState.GameOver)
        {
            //uses the players score to speed up gameplay thus increasing difficulty
            // max at sttart, for each point it is reduced by a factor of 25 until min is reached
            int delay = Math.Max(minDelay,maxDelay-(gameState.Score*delayDecrease));

            await Task.Delay(delay);
            gameState.moveBlockDown();
            draw(gameState);
        }

        GameOverMenu.Visibility =Visibility.Visible;
        FinaleScoreText.Text = $"Final Score: {gameState.Score}";
    }
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {

        //if game is over do nothing
        if (gameState.GameOver)
        {
            return;
        }

        switch (e.Key) 
        {
            case Key.Left:
                gameState.moveBlockLeft();
                break;
            case Key.Right:
                gameState.moveBlockRight();
                break;
            case Key.Down:
                gameState.moveBlockDown();
                break;

            //up for cw rotation
            case Key.Up:
                gameState.rotateBlockCW();
                break;
            //Z for ccw rotation
            case Key.Z:
                gameState.rotateBlockCCW();
                break;
            case Key.C:
                gameState.holdBlock();
                break;
            case Key.Space:
                gameState.dropBlock();
                break;
            default:
                return;
        }

        draw(gameState);


    }

    private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
    {
        //draw method is called once game_canvas is loaded
        await gameLoop();
    }

    private async void PlayAgain_Click(object sender, RoutedEventArgs e)
    {
        //reset gamestate
        gameState = new GameState();

        //hide game over menu
        GameOverMenu.Visibility = Visibility.Hidden;

        //start new game loop
        gameLoop();
    }
}