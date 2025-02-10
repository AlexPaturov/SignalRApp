using System.Windows;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace WPFClient;

public partial class MainWindow : Window
{
    HubConnection connection;

    public MainWindow()
    {
        InitializeComponent();
        connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7213/chathub")
            .WithAutomaticReconnect()
            .Build();

        // проработать подписку на события
        connection.Reconnecting += (sender) =>
        {
            this.Dispatcher.Invoke(() =>
            {
                var newMessage = "Attempting to reconnect...";
                messages.Items.Add(newMessage);
            });

            return Task.CompletedTask;
        };

        connection.Reconnected += (sender) =>
        {
            this.Dispatcher.Invoke(() =>
            {
                var newMessage = "Reconnected to the server...";
                messages.Items.Clear();
                messages.Items.Add(newMessage);
            });

            return Task.CompletedTask;
        };

        connection.Closed += (sender) =>
        {
            this.Dispatcher.Invoke(() =>
            {
                var newMessage = "Connection Closed";
                messages.Items.Add(newMessage);         // ListBox на UI
                openConnection.IsEnabled = true;        // кнопки на UI
                sendMessage.IsEnabled = false;          // кнопки на UI
            });

            return Task.CompletedTask;
        };

    }

    private async void openConnection_Click(object sender, RoutedEventArgs e)
    {
        connection.On<string, string>("ReceiveMessage", (user, message) =>
        {
            this.Dispatcher.Invoke(() =>
            {
                var newMessage = $"{user}: {message}";
                messages.Items.Add(newMessage);         // ListBox на UI
            });
        });

        try
        {
            await connection.StartAsync();
            messages.Items.Add("Connection Started");
            openConnection.IsEnabled = false;
            sendMessage.IsEnabled = true;
        }
        catch (Exception ex) 
        {
            messages.Items.Add(ex.Message);
            openConnection.IsEnabled = true;
            sendMessage.IsEnabled = false;
        }
    }

    private void sendMessage_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            connection.InvokeAsync("SendMessage", "WPF Client", messageInput.Text);
        } 
        catch (Exception ex) 
        {
            messages.Items.Add(ex.Message);
            openConnection.IsEnabled = true;
            sendMessage.IsEnabled = false;
        }
    }

}