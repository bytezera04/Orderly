window.tagifyInterop = {
    init: (elementId, initialTags, maxTags) => {
        const input = document.getElementById(elementId);
        if (!input) {
            return;
        }

        if (input._tagify) {
            input._tagify.destroy();
        }

        const tagify = new Tagify(input, {
            whitelist: [],
            dropdown: { enabled: 0 },
            maxTags: maxTags
        });

        tagify.addTags(initialTags || []);
        input._tagify = tagify;
    },

    initReadonly: (elementId, tags) => {
        const input = document.getElementById(elementId);
        if (!input) return;

        if (input._tagify) {
            input._tagify.destroy();
        }

        const tagify = new Tagify(input, {
            editTags: false,
            userInput: false,
            whitelist: tags || [],
            dropdown: { enabled: 0 },
            callbacks: {
                add: () => false,
                remove: () => false
            }
        });

        tagify.addTags(tags || []);
        input._tagify = tagify;

        // Disable tag removal buttons

        tagify.DOM.scope.querySelectorAll('.tagify__tag__removeBtn')
            .forEach(btn => btn.style.display = 'none');
    },

    getTags: (elementId) => {
        const input = document.getElementById(elementId);
        return input && input._tagify
            ? input._tagify.value.map(t => t.value)
            : [];
    }
};
