# BackgroundQ

Simplifies background processing of data inside an application.
Library code is based on [IHostedService documentation](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/multi-container-microservice-net-applications/background-tasks-with-ihostedservice)

1. Define message class that will be added to the queue and subsequently processed

    ```c#
    public class Message
    {
        public int MessageId { get; set; }
        public string MessageText { get; set; }
    }
    ```
 
1. Implement message processsor that will be called when a new message is added to the queue.
    ```c#
    //This class will be registered as singleton.
    //Messages are processed in order they are added.
    //Messages are processed one by one.

    public class Processor : IBackgroundQProcessor<Message>
    {
        public Processor()
        {
            //add processsor initialization here.
            //you can use DI for required services
        }
        public async Task ProcessQElementAsync(Message message)
        {
            //add processing code here            
        }
    }
    ```
1. Register all classes in ```Startup```
    ```c#
    public void ConfigureServices(IServiceCollection services)
    {
        //This extension method registers Processor as singleton,  IBackgroundQ<Message> as singleton and a HostedService released by the queue when message is added.
        services.AddQService<Message, Processor>();
    }
    ```
1. Inject queue into any of your own services and add an element to it. It will processed automatically in the background.
    ```c#
    public class SomeService
    {
        private readonly IBackgroundQ<Message> _queue;

        public SomeService(IBackgroundQ<Message> queue)
        {
            _queue = queue;
        }

        public void ServiceMethod()
        {
            //do some work
            //and leave message for background processing

            _queue.AddElement(new Message()
                                    {
                                        MessageId = 1,
                                        MessageText = "This is text for message 1"
                                    }
                              );
        }
    }
    ```
