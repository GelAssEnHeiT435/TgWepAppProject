//TODO: edit class structure
class User {
    constructor(id = 0, name = '', age = 0) {
        this.id = id;
        this.name = name;
        this.age = age;
    }

    static fromJson(json) {
        return new User(
            json.id || 0,
            json.name || '',
            json.age || 0
        );
    }

    isValid() {
        return this.id > 0 && 
               this.name && this.name.length > 0 ;
    }

    toJson() {
        return {
            id: this.id,
            name: this.name,
            age: this.age
        };
    }
}