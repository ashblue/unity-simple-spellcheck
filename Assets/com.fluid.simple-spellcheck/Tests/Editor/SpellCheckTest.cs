using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.Fluid.SimpleSpellcheck {
    public class SpellCheckTest {
        public class GetErrorsMethod {
            private IEnglishDictionary _dic;
            private SpellCheckInternal _spellCheck;

            [SetUp]
            public void Setup () {
                _dic = Substitute.For<IEnglishDictionary>();
                _spellCheck = new SpellCheckInternal(_dic);
            }

            [Test]
            public void It_should_return_the_word_in_an_array () {
                var result = _spellCheck.Validate("Lorem");

                Assert.AreEqual("Lorem", result[0].Text);
            }

            [Test]
            public void It_should_return_an_error_for_a_mispelled_word () {
                var result = _spellCheck.Validate("Lorem");

                Assert.AreEqual(false, result[0].IsValid);
            }

            [Test]
            public void It_should_return_success_for_a_properly_spelled_word () {
                _dic.HasWord(Arg.Any<string>()).Returns(true);

                var result = _spellCheck.Validate("Lorem");

                Assert.AreEqual(true, result[0].IsValid);
            }
        }

        public class GetWordsMethod {
            [Test]
            public void It_should_return_true_if_word_is_invalid () {
                var dic = Substitute.For<IEnglishDictionary>();
                var spellCheck = new SpellCheckInternal(dic);

                Assert.IsTrue(spellCheck.IsInvalid("lorem"));
            }

            [Test]
            public void It_should_return_false_if_word_is_valid () {
                var dic = Substitute.For<IEnglishDictionary>();
                dic.HasWord(Arg.Any<string>()).Returns(true);
                var spellCheck = new SpellCheckInternal(dic);

                Assert.IsFalse(spellCheck.IsInvalid("lorem"));

            }
        }
    }
}
