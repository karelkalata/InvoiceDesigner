window.resizeObserver = {
    observers: {},
    observe: function (elementId, dotNetHelper) {
        const element = document.getElementById(elementId);
        if (!element) {
            console.warn(`Element with id ${elementId} not found`);
            return;
        }

        if (this.observers[elementId]) {
            this.observers[elementId].disconnect();
        }

        const observer = new ResizeObserver(entries => {
            for (let entry of entries) {
                let { width, height } = entry.contentRect;
                const target = entry.target;
                console.log("---------------");
                console.log(target.parentElement.style.width);

                //width = Math.round(width / 20) * 20;
                //height = Math.round(height / 20) * 20;

                console.log("start width: " + width);

                width0 = target.parentElement.style.width
                //width0 = width < 300 ? 300 : width;
                height0 = height < 20 ? 20 : height;

                console.log("end width0: " + width0);

                

                target.style.width = `${width0}px`;
                target.style.height = `${height0}px`;

                console.log(target.style.width);
                
                console.log(target);

                dotNetHelper.invokeMethodAsync("OnResize", target.id, width0, height0);
            }
        });

        observer.observe(element);
        this.observers[elementId] = observer;
        },
    unobserve: function (elementId) {
        if (this.observers[elementId]) {
            this.observers[elementId].disconnect();
            delete this.observers[elementId];
        }
    }
};
