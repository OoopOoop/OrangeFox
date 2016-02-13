using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SoundChooserToUniversal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private SoundViewModel soundViewModel;
        private SoundGroup soundGroup;
        private SoundData playingSound;
       

       
      
        public MainPage()
        {
            InitializeComponent();
        }




       async protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is SoundViewModel )
            {
                soundViewModel = e.Parameter as SoundViewModel;
                soundGroup = new SoundGroup();



                // check if json file is not empty
               // customMusicGroup = new CustomMusicGroup();


                await soundViewModel.GetCustomsounds();
            }

            base.OnNavigatedTo(e);
        }
        


     
        //!!! replace SoundData
         private void playMusicButton_Click(object sender, RoutedEventArgs e)
        {
            SoundData data = (sender as Button)?.DataContext as SoundData;
            if (data == null)
            {
                return;
            }

            if (AudioPlayer.CurrentState==Windows.UI.Xaml.Media.MediaElementState.Playing&&data==playingSound)
            {
                AudioPlayer.Stop();
                return;
            }
              
            playingSound = data;


            //check if filetypeEnum is uri or custom, if type is custom ---> get file from FutureAccessList, using metadata

            if (data.fileType == SoundData.FileTypeEnum.Uri)
            {
                AudioPlayer.Source = new Uri(data.FilePath, UriKind.RelativeOrAbsolute);  
            }
            else
            {
                if (data.Stream!=null)
                {
                    AudioPlayer.SetSource(data.Stream, "");
                }
                
                else
                {
                    PlayCustomSong();
                }
            }
        }




        private async void PlayCustomSong()
        {
            foreach  (AccessListEntry item in StorageApplicationPermissions.FutureAccessList.Entries)
            {
                if (item.Metadata == "customTest")
                {
                    StorageFile actualFile = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(item.Token);
//C:\Users\Nastia\OneDrive\Documents\GitHub\OrangeFox\SoundChooserToUniversal\SoundChooserToUniversal\MainPage.xaml
                    var stream = await actualFile.OpenAsync(FileAccessMode.Read);
                    AudioPlayer.SetSource(stream, actualFile.ContentType);
                }
            }
        }




        async private void chooseMusicButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker filePicker = new FileOpenPicker();
            filePicker.SuggestedStartLocation = PickerLocationId.HomeGroup;
            filePicker.ViewMode = PickerViewMode.List;

            filePicker.FileTypeFilter.Clear();
            filePicker.FileTypeFilter.Add(".mp3");
            filePicker.FileTypeFilter.Add(".wav");
            filePicker.FileTypeFilter.Add(".wma");
            filePicker.FileTypeFilter.Add(".qcp");
            filePicker.FileTypeFilter.Add(".m4r");
            filePicker.FileTypeFilter.Add(".m4a");
            filePicker.FileTypeFilter.Add(".aac");


            StorageFile file = await filePicker.PickSingleFileAsync();

            if (file != null)
            {
                var stream = await file.OpenAsync(FileAccessMode.Read);


                // adding chosen songs to CustomMusicCollection
                // soundViewModel.AddCustomSongToCollection(file, stream);

                //without stream
                soundViewModel.AddCustomSongToCollection(file);


                var listToken = StorageApplicationPermissions.FutureAccessList.Add(file, "customTest");


                

                // make "pick a file" collection visible in pop-up menu
                soundViewModel.IsVisible = true;


               //check if song was chosen than save it using SoundModel.SaveCustomLastSong() method
               await  soundViewModel.SaveCustomLastSong();

            }


            else
            {

            }
        }



        private void SoundsList_Unloaded(object sender, RoutedEventArgs e)
        {
            AudioPlayer.Stop();
        }



        private void soundTitletxt_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            SoundData data = (sender as TextBlock)?.DataContext as SoundData;
            soundViewModel.DisplayDescription = data.Title;
            AudioPlayer.Stop();
            musicFlyout.Hide();
        }

        private void musicFlyout_Opening(object sender, object e)
        {
            if (child.ActualWidth == 0 && child.ActualHeight == 0)
            {
                return;
            }

            //double ActualHorizontalOffset = this.musicFlyout.h.HorizontalOffset;
            //double ActualVerticalOffset = this.testPopUp.VerticalOffset;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            test.ShowAt(LayoutRoot);
        }
    }
}
