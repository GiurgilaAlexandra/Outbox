using MassTransit;

namespace POC.Outbox.WebAPI
{

    internal class OutboxConsumer : IConsumer<Models.DB.Outbox>
    {
        public async Task Consume(ConsumeContext<Models.DB.Outbox> context)
        {
            Console.WriteLine("Received request for order " + context.Message.Id);
            await Finish();
        }

        private Task Finish()
        {
            return Task.CompletedTask;
        }
    }
}
