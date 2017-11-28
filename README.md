# Urchin
Urchin is a symmetric n-bit block cipher with extreme disdain for constants.

#### Disclaimer
This project is not intended for production use. It is a for-fun library built to add to my experiences.

#### Usage
Simply build and import the library into your project. From there it's as easy as using the provided API. Here is an example:
```cs
using Urchin;
Cipher cipher = new Cipher 
{
	Key = key,  // your key
	IV = iv    // your IV (basically a salt)
};
// encrypts a string
byte[] encrypted = cipher.Encrypt("hello");
// decrypts a byte array
byte[] decrypted = cipher.Decrypt(encrypted);
// convert back to string
string original = Encoding.ASCII.GetString(decrypted);
// displays "hello"
Console.WriteLine(original);
```

#### What's it for?
It's primarily built for encrypting small snippets of text. A messaging app would be an ideal use case.

#### How does it behave?
Urchin is slow by design (yeah... sure). It's meant to be difficult to brute force. It has no set number of rounds, block, or words sizes; everything is pseudo-randomly selected. Pseudo-random entropy is created by using existing hash algorithms and hashing recursively. The set of hashes used is shuffled after each round.

During each round a block is broken down into words of a pseudo-randomly selected size and passed through an algorithm that transposes or modifies the bits in that word. The algorithms that transform & transpose are also in a pseudo-random order, and are continuously shuffled.

#### Regrets
I gave myself a week to do this project, which had originally been postponed for more than a year. I would have liked to have built a Feistel style cipher a la Bruce Schneier's Blowfish. Perhaps a 2.0 version could be worked out in the future.