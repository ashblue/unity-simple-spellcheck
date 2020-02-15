using System.Collections.Generic;
using NUnit.Framework;

namespace CleverCrow.Fluid.SimpleSpellcheck {
    public class EnglishDictionaryTest {
        private EnglishDictionary Setup (HashSet<string> words = null) {
            if (words == null) words = new HashSet<string>();
            return new EnglishDictionary(words);
        }

        public class HasMethod : EnglishDictionaryTest {
            [Test]
            public void It_should_return_true_if_it_has_a_word () {
                var words = new HashSet<string> { "hello" };
                var dic = Setup(words);

                Assert.IsTrue(dic.HasWord("hello"));
            }

            [Test]
            public void It_should_return_false_if_it_does_not_have_a_word () {
                var dic = Setup();

                Assert.IsFalse(dic.HasWord("hello"));
            }

            [Test]
            public void It_should_not_care_about_case () {
                var words = new HashSet<string> { "hello" };
                var dic = Setup(words);

                Assert.IsTrue(dic.HasWord("Hello"));
            }

            [Test]
            public void It_should_not_care_about_trailing_commas () {
                var words = new HashSet<string> { "hello" };
                var dic = Setup(words);

                Assert.IsTrue(dic.HasWord("Hello,"));
            }

            [Test]
            public void It_should_not_care_about_periods () {
                var words = new HashSet<string> { "hello" };
                var dic = Setup(words);

                Assert.IsTrue(dic.HasWord("hello!"));
            }

            [Test]
            public void It_should_return_true_for_possessive_tense () {
                var words = new HashSet<string> { "duck" };
                var dic = Setup(words);

                Assert.IsTrue(dic.HasWord("Duck's"));
            }

            [Test]
            public void It_should_treat_dashes_as_two_separate_words_for_validation () {
                var words = new HashSet<string> { "my", "duck" };
                var dic = Setup(words);

                Assert.IsTrue(dic.HasWord("my-duck"));
            }

            [Test]
            public void It_should_fail_if_dashes_have_a_missing_word () {
                var words = new HashSet<string> { "my" };
                var dic = Setup(words);

                Assert.IsFalse(dic.HasWord("my-duck"));
            }

            [Test]
            public void It_should_handle_contractions_as_expected () {
                var words = new HashSet<string> { "you've" };
                var dic = Setup(words);

                Assert.IsTrue(dic.HasWord("you've"));
            }

            [Test]
            public void It_should_evaluate_contractions_properly () {
                var words = new HashSet<string> { "i'm" };
                var dic = Setup(words);

                Assert.IsTrue(dic.HasWord("I'm"));
            }
        }
    }
}

