class Counter extends HTMLElement {
    connectedCallback() {
        if (this.dataset.template) {
            const template = document.getElementById(this.dataset.template);
            const shadowRoot = this.attachShadow({mode: "open"});
            shadowRoot.appendChild(template.content.cloneNode(true));
            const button = shadowRoot.querySelector("button");

            button.addEventListener("click", () => {
                console.log(button);
                this.innerText = (parseInt(this.innerText) + 1).toString();
            });

        } else {
            const button = this.querySelector("button");
            button.addEventListener("click", () => {
                console.log(button);
                button.innerText = (parseInt(button.innerText) + 1).toString();
            });
        }
    }
}

customElements.define("my-counter", Counter);