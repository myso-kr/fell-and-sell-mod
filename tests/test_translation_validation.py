import importlib.util
from pathlib import Path
import unittest


spec = importlib.util.spec_from_file_location(
    "validation", Path(__file__).resolve().parents[1] / "tools/check-translations.py"
)
validation = importlib.util.module_from_spec(spec)
spec.loader.exec_module(validation)


class TranslationValidationTests(unittest.TestCase):
    def test_complete_coverage_rejects_missing_entries(self):
        source = {"UI/1": {"text": "Back"}}
        self.assertEqual([], validation.check(source, {}))
        self.assertEqual(["Missing translation: UI/1"], validation.check(source, {}, True))
        self.assertEqual([], validation.check(source, {"UI/1": "뒤로"}, True))

    def test_preserves_reordered_placeholders(self):
        source = {"UI/1": {"text": "Slot {0}: {1}"}}
        self.assertEqual([], validation.check(source, {"UI/1": "{1} — 슬롯 {0}"}))

    def test_rejects_lost_repeated_token(self):
        source = {"Game/1": {"text": "{Inventory} then {Inventory}"}}
        self.assertTrue(validation.check(source, {"Game/1": "소지품 {Inventory}"}))

    def test_rejects_rich_text_and_button_changes(self):
        source = {"UI/1": {"text": "<color=#fff>[ESC] {0}</color>"}}
        self.assertTrue(validation.check(source, {"UI/1": "<color=#000>[ESC] {0}</color>"}))
        self.assertTrue(validation.check(source, {"UI/1": "<color=#fff>[E] {0}</color>"}))

    def test_rejects_unknown_and_empty_entries(self):
        self.assertTrue(validation.check({}, {"UI/1": "뒤로"}))
        self.assertTrue(validation.check({"UI/1": {"text": "Back"}}, {"UI/1": " "}))


if __name__ == "__main__":
    unittest.main()
