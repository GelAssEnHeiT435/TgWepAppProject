//TODO: edit class structure
class User {
    constructor(id = 0, name = '') {
        this.id = id;
        this.name = name;
    }

    static fromJson(json) {
        return new User(
            json.id || 0,
            json.name || ''
        );
    }

    isValid() {
        return this.id > 0 && 
               this.name && this.name.length > 0 ;
    }

    static toJson() {
        return {
            id: this.id,
            name: this.name
        };
    }
}