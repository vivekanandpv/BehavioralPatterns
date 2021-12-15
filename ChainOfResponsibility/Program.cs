using System;

namespace ChainOfResponsibility {
    class Program {
        static void Main(string[] args) {
            //  Section 6
            //  Create a pipeline first (optional)
            MiddlewareBase pipeline = new Pipeline();

            //  Attaching handler middleware (conventional approach)
            pipeline
                .AddMiddleware(new LoggingMiddleware())
                .AddMiddleware(new LoggingMiddleware())
                .AddMiddleware(new ResponseMiddleware());

            //  Request creation
            IRequest request = new Request("Request from client");

            //  Section 7
            //  Passing the request to the pipeline
            //  The crux: client doesn't know the exact handler
            //  Request maker and handler are completely decoupled
            Console.WriteLine(pipeline.Handle(request).ResponseMessage);
        }
    }

    //  Section 1
    //  Request and Response interfaces and implementations
    //  Client sends the requests and one of the handler objects handles
    //  Handler may optionally return a response.
    interface IRequest {
        string Message { get; }
    }

    class Request : IRequest {
        public Request(string message) {
            Message = message;
        }

        public string Message { get; }
    }

    interface IResponse {
        string ResponseMessage { get; }
    }

    class Response : IResponse {
        public Response(string responseMessage) {
            ResponseMessage = responseMessage;
        }

        public string ResponseMessage { get; }
    }

    //  Section 2
    //  Handler object may colloquially be called a middleware
    //  This base class sets up the interface for such handlers
    //  The key idea is, the handler makes the decision whether to handle the request or pass it on to the next in line
    //  Handler object per se is unaware of the next handler
    //  There are several variations. Here I chose to respond in the base if none of the handlers handle.
    //  Handlers to the chain can be added at runtime
    abstract class MiddlewareBase {
        private MiddlewareBase _nextMiddleware;

        //  Supplying next-in-line handler at the instantiation is optional
        protected MiddlewareBase(MiddlewareBase nextMiddleware = null) {
            _nextMiddleware = nextMiddleware;
        }
        
        //  When handler objects don't want to handle or there are no handlers
        public virtual IResponse Handle(IRequest request) {
            if (_nextMiddleware != null) {
                return _nextMiddleware.Handle(request);
            }

            //  You may consider throwing an exception here
            return new Response("Default Response: No middleware handled this request");
        }

        public MiddlewareBase AddMiddleware(MiddlewareBase middleware) {
            var current = this;

            while (current._nextMiddleware != null) {
                current = current._nextMiddleware;
            }

            current._nextMiddleware = middleware;

            return this;    //  for fluent API style
        }
    }

    //  Section 3
    //  This is optional
    //  The idea is to have a notion of pipeline on which handler middleware are attached
    class Pipeline : MiddlewareBase {
        public Pipeline(MiddlewareBase nextMiddleware) : base(nextMiddleware) {
        }

        public Pipeline() {
        }
    }

    class LoggingMiddleware : MiddlewareBase {
        public LoggingMiddleware(MiddlewareBase nextMiddleware) : base(nextMiddleware) {
        }

        public LoggingMiddleware() {
        }

        public override IResponse Handle(IRequest request) {
            Console.WriteLine($"Logging middleware: {request.Message}");
            //  Section 4
            //  This middleware intercepts the request but doesn't handle
            //  So, passing on
            return base.Handle(request);
        }
    }

    class ResponseMiddleware : MiddlewareBase {
        public ResponseMiddleware(MiddlewareBase nextMiddleware) : base(nextMiddleware) {
        }

        public ResponseMiddleware() {
        }

        public override IResponse Handle(IRequest request) {
            //  Section 5
            //  A terminal middleware; produces response
            return new Response($"ResponseMiddleware: handled the request: {request.Message}");
        }
    }


}
