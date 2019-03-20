using System;

namespace Catalog.BusinessLogicLayer {
    public interface IExceptionHandler {
        void Handle(Exception exception);
    }
}